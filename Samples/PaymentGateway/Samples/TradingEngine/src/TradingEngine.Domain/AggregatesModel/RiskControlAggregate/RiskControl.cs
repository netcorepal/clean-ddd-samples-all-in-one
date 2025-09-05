using TradingEngine.Domain.DomainEvents;
using TradingEngine.Domain.AggregatesModel.TradeAggregate;

namespace TradingEngine.Domain.AggregatesModel.RiskControlAggregate;

public partial record RiskControlId : IGuidStronglyTypedId;
public partial record RiskAssessmentId : IGuidStronglyTypedId;

public enum RiskLevel
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

public enum RiskType
{
    PositionLimit = 1,
    DailyLossLimit = 2,
    ConcentrationRisk = 3,
    LiquidityRisk = 4,
    MarketRisk = 5
}

/// <summary>
/// 风险控制聚合根
/// </summary>
public class RiskControl : Entity<RiskControlId>, IAggregateRoot
{
    protected RiskControl() { }

    public RiskControl(string userId, decimal totalPositionLimit, decimal dailyLossLimit)
    {
        UserId = userId;
        TotalPositionLimit = totalPositionLimit;
        DailyLossLimit = dailyLossLimit;
        CurrentPosition = 0;
        DailyLoss = 0;
        IsActive = true;
        CreatedAt = DateTimeOffset.UtcNow;
        
        this.AddDomainEvent(new RiskControlCreatedDomainEvent(this));
    }

    #region Properties

    public string UserId { get; private set; } = string.Empty;
    public decimal TotalPositionLimit { get; private set; }
    public decimal DailyLossLimit { get; private set; }
    public decimal CurrentPosition { get; private set; }
    public decimal DailyLoss { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? LastAssessmentAt { get; private set; }
    public RowVersion RowVersion { get; private set; } = new RowVersion(0);

    private readonly List<RiskAssessment> _riskAssessments = new();
    public IReadOnlyCollection<RiskAssessment> RiskAssessments => _riskAssessments;

    #endregion

    #region Methods

    public RiskAssessment AssessTradeRisk(string symbol, decimal quantity, decimal price, TradeType tradeType)
    {
        var riskLevel = RiskLevel.Low;
        var riskTypes = new List<RiskType>();
        var description = new List<string>();

        // 检查持仓限制
        var positionValue = quantity * price;
        if (CurrentPosition + positionValue > TotalPositionLimit)
        {
            riskLevel = RiskLevel.High;
            riskTypes.Add(RiskType.PositionLimit);
            description.Add($"Position would exceed limit. Current: {CurrentPosition}, Adding: {positionValue}, Limit: {TotalPositionLimit}");
        }

        // 检查集中度风险（单一标的超过总仓位的30%）
        var concentrationLimit = TotalPositionLimit * 0.3m;
        if (positionValue > concentrationLimit)
        {
            if (riskLevel < RiskLevel.Medium) riskLevel = RiskLevel.Medium;
            riskTypes.Add(RiskType.ConcentrationRisk);
            description.Add($"High concentration in single symbol: {positionValue} > {concentrationLimit}");
        }

        // 检查流动性风险（大额交易）
        if (positionValue > 1000000) // 100万以上认为是大额交易
        {
            if (riskLevel < RiskLevel.Medium) riskLevel = RiskLevel.Medium;
            riskTypes.Add(RiskType.LiquidityRisk);
            description.Add($"Large trade amount: {positionValue}");
        }

        var assessment = new RiskAssessment(symbol, quantity, price, tradeType, riskLevel, riskTypes, string.Join("; ", description));
        _riskAssessments.Add(assessment);
        
        LastAssessmentAt = DateTimeOffset.UtcNow;
        
        this.AddDomainEvent(new RiskAssessmentCreatedDomainEvent(this, assessment));
        
        return assessment;
    }

    public void UpdatePosition(decimal positionChange)
    {
        CurrentPosition += positionChange;
        this.AddDomainEvent(new PositionUpdatedDomainEvent(this, positionChange));
    }

    public void UpdateDailyLoss(decimal lossAmount)
    {
        DailyLoss += lossAmount;
        
        if (DailyLoss > DailyLossLimit)
        {
            this.AddDomainEvent(new DailyLossLimitExceededDomainEvent(this, DailyLoss, DailyLossLimit));
        }
    }

    public void ResetDailyLoss()
    {
        DailyLoss = 0;
        this.AddDomainEvent(new DailyLossResetDomainEvent(this));
    }

    public void Activate()
    {
        IsActive = true;
        this.AddDomainEvent(new RiskControlActivatedDomainEvent(this));
    }

    public void Deactivate()
    {
        IsActive = false;
        this.AddDomainEvent(new RiskControlDeactivatedDomainEvent(this));
    }

    #endregion
}

/// <summary>
/// 风险评估子实体
/// </summary>
public class RiskAssessment : Entity<RiskAssessmentId>
{
    protected RiskAssessment() { }

    public RiskAssessment(string symbol, decimal quantity, decimal price, TradeType tradeType, 
        RiskLevel riskLevel, List<RiskType> riskTypes, string description)
    {
        Symbol = symbol;
        Quantity = quantity;
        Price = price;
        TradeType = tradeType;
        RiskLevel = riskLevel;
        RiskTypes = riskTypes;
        Description = description;
        AssessedAt = DateTimeOffset.UtcNow;
    }

    public string Symbol { get; private set; } = string.Empty;
    public decimal Quantity { get; private set; }
    public decimal Price { get; private set; }
    public TradeType TradeType { get; private set; }
    public RiskLevel RiskLevel { get; private set; }
    public List<RiskType> RiskTypes { get; private set; } = new();
    public string Description { get; private set; } = string.Empty;
    public DateTimeOffset AssessedAt { get; private set; }
}
