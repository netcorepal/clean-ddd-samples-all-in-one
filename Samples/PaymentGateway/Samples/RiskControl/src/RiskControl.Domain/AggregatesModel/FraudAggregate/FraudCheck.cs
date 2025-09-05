using RiskControl.Domain.AggregatesModel.OrderAggregate;
using RiskControl.Domain.DomainEvents;

namespace RiskControl.Domain.AggregatesModel.FraudAggregate;

public partial record FraudCheckId : IGuidStronglyTypedId;

/// <summary>
/// 反欺诈检测记录
/// </summary>
public class FraudCheck : Entity<FraudCheckId>, IAggregateRoot
{
    protected FraudCheck() { }

    public FraudCheck(OrderId orderId, string channel, decimal amount, string ipAddress)
    {
        OrderId = orderId;
        Channel = channel;
        Amount = amount;
        IpAddress = ipAddress;
        CheckedAt = DateTimeOffset.UtcNow;
        Status = "Pending";
    }

    #region Properties
    public OrderId OrderId { get; private set; } = default!;
    public string Channel { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public string IpAddress { get; private set; } = string.Empty;
    public int RiskScore { get; private set; }
    public string Result { get; private set; } = string.Empty; // Pass/Review/Reject
    public string Status { get; private set; } = string.Empty; // Pending/Completed
    public string Reasons { get; private set; } = string.Empty;
    public DateTimeOffset CheckedAt { get; private set; }
    public RowVersion RowVersion { get; private set; } = new RowVersion();
    #endregion

    #region Methods
    public void Complete(int riskScore, string result, string reasons)
    {
        if (Status == "Completed")
        {
            throw new KnownException("Fraud check already completed");
        }
        RiskScore = riskScore;
        Result = result;
        Reasons = reasons;
        Status = "Completed";
        this.AddDomainEvent(new FraudCheckCompletedDomainEvent(this));
    }
    #endregion
}
