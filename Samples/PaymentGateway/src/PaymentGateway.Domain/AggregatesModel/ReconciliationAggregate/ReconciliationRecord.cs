using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;

namespace PaymentGateway.Domain.AggregatesModel.ReconciliationAggregate;

public enum ReconciliationStatus
{
    Pending = 0,
    Matched = 1,
    Mismatch = 2,
}

public partial record ReconciliationRecordId : IInt64StronglyTypedId;

public class ReconciliationRecord : Entity<ReconciliationRecordId>, IAggregateRoot
{
    protected ReconciliationRecord() { }

    public ReconciliationRecord(string provider, string providerTransactionId, decimal amount, string currency, DateTimeOffset occurTime)
    {
        Provider = provider;
        ProviderTransactionId = providerTransactionId;
        Amount = amount;
        Currency = currency;
        OccurTime = occurTime;
        Status = ReconciliationStatus.Pending;
    }

    public string Provider { get; private set; } = string.Empty;
    public string ProviderTransactionId { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public DateTimeOffset OccurTime { get; private set; }
    public PaymentId? PaymentId { get; private set; }
    public ReconciliationStatus Status { get; private set; } = ReconciliationStatus.Pending;
    public string? Note { get; private set; }
    public RowVersion RowVersion { get; private set; } = new RowVersion();

    public void MarkMatched(PaymentId paymentId)
    {
        PaymentId = paymentId;
        Status = ReconciliationStatus.Matched;
    }

    public void MarkMismatch(string note)
    {
        Note = note;
        Status = ReconciliationStatus.Mismatch;
    }
}
