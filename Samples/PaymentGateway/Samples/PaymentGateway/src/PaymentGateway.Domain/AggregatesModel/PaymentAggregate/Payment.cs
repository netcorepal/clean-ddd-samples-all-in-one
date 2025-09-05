using PaymentGateway.Domain.AggregatesModel.OrderAggregate;
using PaymentGateway.Domain.DomainEvents;

namespace PaymentGateway.Domain.AggregatesModel.PaymentAggregate;

public enum PaymentChannel
{
    Unknown = 0,
    WeChat = 1,
    Alipay = 2,
    Stripe = 3,
    PayPal = 4,
}

public enum PaymentStatus
{
    Initiated = 0,
    Pending = 1,
    Succeeded = 2,
    Failed = 3,
}

public partial record PaymentId : IInt64StronglyTypedId;

public class Payment : Entity<PaymentId>, IAggregateRoot
{
    protected Payment() { }

    public Payment(OrderId orderId, decimal amount, string currency, PaymentChannel channel)
    {
        OrderId = orderId;
        Amount = amount;
        Currency = currency;
        Channel = channel;
        Status = PaymentStatus.Initiated;
        CreatedTime = DateTimeOffset.UtcNow;
        this.AddDomainEvent(new PaymentInitiatedDomainEvent(this));
    }

    public OrderId OrderId { get; private set; } = default!;
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public PaymentChannel Channel { get; private set; }
    public PaymentStatus Status { get; private set; } = PaymentStatus.Initiated;
    public string? ProviderTransactionId { get; private set; }
    public DateTimeOffset CreatedTime { get; private set; }
    public DateTimeOffset? SucceededTime { get; private set; }
    public DateTimeOffset? FailedTime { get; private set; }
    public RowVersion RowVersion { get; private set; } = new RowVersion();

    public void MarkPending()
    {
        if (Status != PaymentStatus.Initiated)
        {
            throw new KnownException("Only initiated payment can be marked pending");
        }
        Status = PaymentStatus.Pending;
    }

    public void SetProviderTransaction(string providerTransactionId)
    {
        if (string.IsNullOrWhiteSpace(providerTransactionId))
        {
            throw new KnownException("providerTransactionId required");
        }
        ProviderTransactionId = providerTransactionId;
    }

    public void MarkSucceeded(string providerTransactionId)
    {
        if (Status == PaymentStatus.Succeeded)
        {
            return; // idempotent
        }
        if (Status == PaymentStatus.Failed)
        {
            throw new KnownException("Failed payment cannot be marked succeeded");
        }

        ProviderTransactionId = providerTransactionId;
        Status = PaymentStatus.Succeeded;
        SucceededTime = DateTimeOffset.UtcNow;
        this.AddDomainEvent(new PaymentSucceededDomainEvent(this));
    }

    public void MarkFailed(string reason)
    {
        if (Status == PaymentStatus.Succeeded)
        {
            throw new KnownException("Succeeded payment cannot be marked failed");
        }
        Status = PaymentStatus.Failed;
        FailedTime = DateTimeOffset.UtcNow;
        this.AddDomainEvent(new PaymentFailedDomainEvent(this, reason));
    }
}
