using RiskControl.Domain.AggregatesModel.OrderAggregate;
using RiskControl.Domain.DomainEvents;

namespace RiskControl.Domain.AggregatesModel.ComplianceAggregate;

public partial record ComplianceAlertId : IGuidStronglyTypedId;

/// <summary>
/// 合规监控告警
/// </summary>
public class ComplianceAlert : Entity<ComplianceAlertId>, IAggregateRoot
{
    protected ComplianceAlert() { }

    public ComplianceAlert(OrderId orderId, string ruleCode, string detail)
    {
        OrderId = orderId;
        RuleCode = ruleCode;
        Detail = detail;
        Status = "Open";
        CreatedAt = DateTimeOffset.UtcNow;
    }

    #region Properties
    public OrderId OrderId { get; private set; } = default!;
    public string RuleCode { get; private set; } = string.Empty;
    public string Detail { get; private set; } = string.Empty;
    public string Status { get; private set; } = string.Empty; // Open/Closed
    public string? Resolution { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ClosedAt { get; private set; }
    public RowVersion RowVersion { get; private set; } = new RowVersion();
    #endregion

    #region Methods
    public void Close(string resolution)
    {
        if (Status == "Closed")
        {
            throw new KnownException("Compliance alert already closed");
        }
        Resolution = resolution;
        Status = "Closed";
        ClosedAt = DateTimeOffset.UtcNow;
        this.AddDomainEvent(new ComplianceAlertClosedDomainEvent(this));
    }
    #endregion
}
