using TradingEngine.Domain.DomainEvents;
using TradingEngine.Domain.AggregatesModel.TradeAggregate;

namespace TradingEngine.Domain.AggregatesModel.SettlementAggregate;

public partial record SettlementId : IGuidStronglyTypedId;
public partial record SettlementItemId : IGuidStronglyTypedId;

public enum SettlementStatus
{
    Pending = 1,
    Processing = 2,
    Completed = 3,
    Failed = 4,
    Cancelled = 5
}

public enum SettlementType
{
    TradeSettlement = 1,
    DividendSettlement = 2,
    FeeSettlement = 3,
    MarginAdjustment = 4
}

/// <summary>
/// 清算结算聚合根
/// </summary>
public class Settlement : Entity<SettlementId>, IAggregateRoot
{
    protected Settlement() { }

    public Settlement(string userId, SettlementType settlementType, decimal totalAmount, DateTimeOffset settlementDate)
    {
        UserId = userId;
        SettlementType = settlementType;
        TotalAmount = totalAmount;
        SettlementDate = settlementDate;
        Status = SettlementStatus.Pending;
        CreatedAt = DateTimeOffset.UtcNow;
        
        this.AddDomainEvent(new SettlementCreatedDomainEvent(this));
    }

    #region Properties

    public string UserId { get; private set; } = string.Empty;
    public SettlementType SettlementType { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTimeOffset SettlementDate { get; private set; }
    public SettlementStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ProcessedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public string? FailureReason { get; private set; }
    public RowVersion RowVersion { get; private set; } = new RowVersion(0);

    private readonly List<SettlementItem> _items = new();
    public IReadOnlyCollection<SettlementItem> Items => _items;

    #endregion

    #region Methods

    public void AddTradeSettlementItem(TradeId tradeId, string symbol, decimal quantity, decimal price, TradeType tradeType)
    {
        if (Status != SettlementStatus.Pending)
        {
            throw new KnownException($"Cannot add items to settlement in {Status} status");
        }

        var amount = tradeType == TradeType.Buy ? -(quantity * price) : (quantity * price);
        var item = new SettlementItem(tradeId.ToString(), symbol, quantity, price, amount, "Trade Settlement");
        _items.Add(item);
        
        RecalculateTotalAmount();
        this.AddDomainEvent(new SettlementItemAddedDomainEvent(this, item));
    }

    public void AddFeeSettlementItem(string description, decimal feeAmount)
    {
        if (Status != SettlementStatus.Pending)
        {
            throw new KnownException($"Cannot add items to settlement in {Status} status");
        }

        var item = new SettlementItem(Guid.NewGuid().ToString(), "FEE", 1, feeAmount, -feeAmount, description);
        _items.Add(item);
        
        RecalculateTotalAmount();
        this.AddDomainEvent(new SettlementItemAddedDomainEvent(this, item));
    }

    public void StartProcessing()
    {
        if (Status != SettlementStatus.Pending)
        {
            throw new KnownException($"Cannot start processing settlement in {Status} status");
        }

        Status = SettlementStatus.Processing;
        ProcessedAt = DateTimeOffset.UtcNow;
        this.AddDomainEvent(new SettlementProcessingStartedDomainEvent(this));
    }

    public void Complete()
    {
        if (Status != SettlementStatus.Processing)
        {
            throw new KnownException($"Cannot complete settlement in {Status} status");
        }

        Status = SettlementStatus.Completed;
        CompletedAt = DateTimeOffset.UtcNow;
        this.AddDomainEvent(new SettlementCompletedDomainEvent(this));
    }

    public void Fail(string reason)
    {
        if (Status != SettlementStatus.Processing)
        {
            throw new KnownException($"Cannot fail settlement in {Status} status");
        }

        Status = SettlementStatus.Failed;
        FailureReason = reason;
        this.AddDomainEvent(new SettlementFailedDomainEvent(this, reason));
    }

    public void Cancel()
    {
        if (Status != SettlementStatus.Pending)
        {
            throw new KnownException($"Cannot cancel settlement in {Status} status");
        }

        Status = SettlementStatus.Cancelled;
        this.AddDomainEvent(new SettlementCancelledDomainEvent(this));
    }

    private void RecalculateTotalAmount()
    {
        TotalAmount = _items.Sum(item => item.Amount);
    }

    #endregion
}

/// <summary>
/// 结算项目子实体
/// </summary>
public class SettlementItem : Entity<SettlementItemId>
{
    protected SettlementItem() { }

    public SettlementItem(string referenceId, string symbol, decimal quantity, decimal price, decimal amount, string description)
    {
        ReferenceId = referenceId;
        Symbol = symbol;
        Quantity = quantity;
        Price = price;
        Amount = amount;
        Description = description;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public string ReferenceId { get; private set; } = string.Empty; // 关联的交易ID或其他引用
    public string Symbol { get; private set; } = string.Empty;
    public decimal Quantity { get; private set; }
    public decimal Price { get; private set; }
    public decimal Amount { get; private set; } // 结算金额（正数表示收入，负数表示支出）
    public string Description { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; private set; }
}
