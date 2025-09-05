using ReportingService.Domain.AggregatesModel.FinancialReportAggregate;

namespace ReportingService.Domain.Tests;

public class FinancialReportTests
{
    [Fact]
    public void CreateFinancialReport_Should_SetProperties()
    {
        var r = new FinancialReport("标题", "2025Q3", "内容");
        Assert.Equal("标题", r.Title);
        Assert.Equal("2025Q3", r.Period);
        Assert.False(r.GeneratedAt == default);
    }
}
