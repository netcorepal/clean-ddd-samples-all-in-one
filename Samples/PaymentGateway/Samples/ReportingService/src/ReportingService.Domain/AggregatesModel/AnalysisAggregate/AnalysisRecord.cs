namespace ReportingService.Domain.AggregatesModel.AnalysisAggregate;

public partial record AnalysisRecordId : IInt64StronglyTypedId;

/// <summary>
/// 数据分析记录
/// </summary>
public class AnalysisRecord : Entity<AnalysisRecordId>, IAggregateRoot
{
    protected AnalysisRecord() { }

    public AnalysisRecord(string name, string? parameters = null)
    {
        Name = name;
        Parameters = parameters ?? string.Empty;
        Status = "Pending";
        StartedAt = DateTimeOffset.UtcNow;
    }

    public string Name { get; private set; } = string.Empty;
    public string Parameters { get; private set; } = string.Empty;
    public string Status { get; private set; } = string.Empty;
    public DateTimeOffset StartedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public string? Result { get; private set; }

    public RowVersion RowVersion { get; private set; } = new RowVersion();
    public UpdateTime UpdateTime { get; private set; } = new UpdateTime(DateTimeOffset.UtcNow);

    public void Complete(string result)
    {
        if (Status == "Completed")
        {
            throw new KnownException("分析已完成");
        }
        Result = result;
        Status = "Completed";
        CompletedAt = DateTimeOffset.UtcNow;
    }
}
