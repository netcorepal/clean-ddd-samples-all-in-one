using ReportingService.Domain.DomainEvents;

namespace ReportingService.Domain.AggregatesModel.RegulatoryReportAggregate;

public partial record RegulatoryReportId : IInt64StronglyTypedId;

/// <summary>
/// 监管报告聚合根
/// </summary>
public class RegulatoryReport : Entity<RegulatoryReportId>, IAggregateRoot
{
    protected RegulatoryReport() { }

    public RegulatoryReport(string category, string period, string? payload = null)
    {
        Category = category;
        Period = period;
        Payload = payload ?? string.Empty;
        Submitted = false;
        this.AddDomainEvent(new RegulatoryReportCreatedDomainEvent(this));
    }

    public string Category { get; private set; } = string.Empty;
    public string Period { get; private set; } = string.Empty;
    public string Payload { get; private set; } = string.Empty;
    public bool Submitted { get; private set; }

    public RowVersion RowVersion { get; private set; } = new RowVersion();
    public UpdateTime UpdateTime { get; private set; } = new UpdateTime(DateTimeOffset.UtcNow);

    public void MarkSubmitted()
    {
        if (Submitted)
        {
            throw new KnownException("报告已提交");
        }
        Submitted = true;
    }
}
