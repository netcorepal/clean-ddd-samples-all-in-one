using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.DomainEvents;

namespace PaymentGateway.Domain.AggregatesModel.RefundAggregate;

public enum RefundStatus
{
    Requested = 0,
    Processing = 1,
    Succeeded = 2,
    Failed = 3,
}

public partial record RefundId : IInt64StronglyTypedId;

public class Refund : Entity<RefundId>, IAggregateRoot
{
    protected Refund() { }

    public Refund(PaymentId paymentId, decimal amount, string reason)
    {
        PaymentId = paymentId;
        Amount = amount;
        Reason = reason;
        Status = RefundStatus.Requested;
        CreatedTime = DateTimeOffset.UtcNow;
        this.AddDomainEvent(new RefundRequestedDomainEvent(this));
    }

    public PaymentId PaymentId { get; private set; } = default!;
    public decimal Amount { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public RefundStatus Status { get; private set; } = RefundStatus.Requested;
    public string? ProviderRefundId { get; private set; }
    public DateTimeOffset CreatedTime { get; private set; }
    public DateTimeOffset? SucceededTime { get; private set; }
    public DateTimeOffset? FailedTime { get; private set; }
    public RowVersion RowVersion { get; private set; } = new RowVersion();

    public void MarkProcessing()
    {
        if (Status != RefundStatus.Requested)
        {
            throw new KnownException("Only requested refund can be marked processing");
        }
        Status = RefundStatus.Processing;
    }

    public void MarkSucceeded(string providerRefundId)
    {
        if (Status == RefundStatus.Succeeded)
        {
            return; // idempotent
        }
        if (Status == RefundStatus.Failed)
        {
            throw new KnownException("Failed refund cannot be marked succeeded");
        }
        ProviderRefundId = providerRefundId;
        Status = RefundStatus.Succeeded;
        SucceededTime = DateTimeOffset.UtcNow;
        this.AddDomainEvent(new RefundSucceededDomainEvent(this));
    }

    public void MarkFailed(string reason)
    {
        if (Status == RefundStatus.Succeeded)
        {
            throw new KnownException("Succeeded refund cannot be marked failed");
        }
        Status = RefundStatus.Failed;
        FailedTime = DateTimeOffset.UtcNow;
        this.AddDomainEvent(new RefundFailedDomainEvent(this, reason));
    }
}
