using ReportingService.Domain.DomainEvents;

namespace ReportingService.Domain.AggregatesModel.FinancialReportAggregate;

public partial record FinancialReportId : IInt64StronglyTypedId;

/// <summary>
/// 财务报表聚合根
/// </summary>
public class FinancialReport : Entity<FinancialReportId>, IAggregateRoot
{
    protected FinancialReport() { }

    public FinancialReport(string title, string period, string? content = null)
    {
        Title = title;
        Period = period;
        Content = content ?? string.Empty;
        GeneratedAt = DateTimeOffset.UtcNow;
        this.AddDomainEvent(new FinancialReportCreatedDomainEvent(this));
    }

    public string Title { get; private set; } = string.Empty;
    public string Period { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public DateTimeOffset GeneratedAt { get; private set; }

    public RowVersion RowVersion { get; private set; } = new RowVersion();
    public UpdateTime UpdateTime { get; private set; } = new UpdateTime(DateTimeOffset.UtcNow);
}
