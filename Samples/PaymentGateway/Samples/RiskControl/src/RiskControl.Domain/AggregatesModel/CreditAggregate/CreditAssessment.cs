using RiskControl.Domain.AggregatesModel.OrderAggregate;
using RiskControl.Domain.DomainEvents;

namespace RiskControl.Domain.AggregatesModel.CreditAggregate;

public partial record CreditAssessmentId : IGuidStronglyTypedId;

/// <summary>
/// 信用评估记录
/// </summary>
public class CreditAssessment : Entity<CreditAssessmentId>, IAggregateRoot
{
    protected CreditAssessment() { }

    public CreditAssessment(OrderId orderId, string customerId, decimal exposure)
    {
        OrderId = orderId;
        CustomerId = customerId;
        Exposure = exposure;
        Status = "Pending";
        AssessedAt = DateTimeOffset.UtcNow;
    }

    #region Properties
    public OrderId OrderId { get; private set; } = default!;
    public string CustomerId { get; private set; } = string.Empty;
    public decimal Exposure { get; private set; }
    public int Score { get; private set; }
    public string Grade { get; private set; } = string.Empty; // AAA/AA/A/BBB/BB/B/C/D
    public string Status { get; private set; } = string.Empty; // Pending/Completed
    public DateTimeOffset AssessedAt { get; private set; }
    public RowVersion RowVersion { get; private set; } = new RowVersion();
    #endregion

    #region Methods
    public void Complete(int score)
    {
        if (Status == "Completed")
        {
            throw new KnownException("Credit assessment already completed");
        }
        Score = score;
        Grade = score switch
        {
            >= 800 => "AAA",
            >= 740 => "AA",
            >= 680 => "A",
            >= 620 => "BBB",
            >= 580 => "BB",
            >= 540 => "B",
            >= 500 => "C",
            _ => "D"
        };
        Status = "Completed";
        this.AddDomainEvent(new CreditAssessmentCompletedDomainEvent(this));
    }
    #endregion
}
