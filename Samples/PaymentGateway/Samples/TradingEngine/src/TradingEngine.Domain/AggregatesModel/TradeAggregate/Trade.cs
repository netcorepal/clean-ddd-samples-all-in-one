using TradingEngine.Domain.DomainEvents;

namespace TradingEngine.Domain.AggregatesModel.TradeAggregate;

public partial record TradeId : IGuidStronglyTypedId;

public enum TradeType
{
    Buy = 1,
    Sell = 2
}

public enum TradeStatus
{
    Pending = 1,
    Executed = 2,
    Failed = 3,
    Cancelled = 4,
    PartiallyFilled = 5
}

/// <summary>
/// 交易聚合根
/// </summary>
public class Trade : Entity<TradeId>, IAggregateRoot
{
    protected Trade() { }

    public Trade(string symbol, TradeType tradeType, decimal quantity, decimal price, string userId)
    {
        Symbol = symbol;
        TradeType = tradeType;
        Quantity = quantity;
        Price = price;
        UserId = userId;
        Status = TradeStatus.Pending;
        ExecutedQuantity = 0;
        RemainingQuantity = quantity;
        TotalValue = quantity * price;
        CreatedAt = DateTimeOffset.UtcNow;
        
        this.AddDomainEvent(new TradeCreatedDomainEvent(this));
    }

    #region Properties

    public string Symbol { get; private set; } = string.Empty;
    public TradeType TradeType { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal Price { get; private set; }
    public decimal ExecutedQuantity { get; private set; }
    public decimal RemainingQuantity { get; private set; }
    public decimal TotalValue { get; private set; }
    public TradeStatus Status { get; private set; }
    public string UserId { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ExecutedAt { get; private set; }
    public string? FailureReason { get; private set; }
    public RowVersion RowVersion { get; private set; } = new RowVersion(0);

    #endregion

    #region Methods

    public void Execute(decimal executedQuantity, decimal executedPrice)
    {
        if (Status != TradeStatus.Pending && Status != TradeStatus.PartiallyFilled)
        {
            throw new KnownException($"Cannot execute trade in {Status} status");
        }

        if (executedQuantity > RemainingQuantity)
        {
            throw new KnownException("Executed quantity cannot exceed remaining quantity");
        }

        ExecutedQuantity += executedQuantity;
        RemainingQuantity -= executedQuantity;
        
        if (RemainingQuantity == 0)
        {
            Status = TradeStatus.Executed;
            ExecutedAt = DateTimeOffset.UtcNow;
            this.AddDomainEvent(new TradeExecutedDomainEvent(this));
        }
        else
        {
            Status = TradeStatus.PartiallyFilled;
            this.AddDomainEvent(new TradePartiallyFilledDomainEvent(this, executedQuantity, executedPrice));
        }
    }

    public void Fail(string reason)
    {
        if (Status != TradeStatus.Pending && Status != TradeStatus.PartiallyFilled)
        {
            throw new KnownException($"Cannot fail trade in {Status} status");
        }

        Status = TradeStatus.Failed;
        FailureReason = reason;
        this.AddDomainEvent(new TradeFailedDomainEvent(this, reason));
    }

    public void Cancel()
    {
        if (Status != TradeStatus.Pending)
        {
            throw new KnownException($"Cannot cancel trade in {Status} status");
        }

        Status = TradeStatus.Cancelled;
        this.AddDomainEvent(new TradeCancelledDomainEvent(this));
    }

    #endregion
}
