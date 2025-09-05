using ReportingService.Domain.AggregatesModel.RegulatoryReportAggregate;

namespace ReportingService.Domain.Tests;

public class RegulatoryReportTests
{
    [Fact]
    public void MarkSubmitted_Should_Set_Flag()
    {
        var r = new RegulatoryReport("CAT", "2025Q3", "{}");
        Assert.False(r.Submitted);
        r.MarkSubmitted();
        Assert.True(r.Submitted);
    }
}
