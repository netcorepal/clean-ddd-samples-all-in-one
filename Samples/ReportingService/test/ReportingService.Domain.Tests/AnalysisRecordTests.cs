using ReportingService.Domain.AggregatesModel.AnalysisAggregate;

namespace ReportingService.Domain.Tests;

public class AnalysisRecordTests
{
    [Fact]
    public void Complete_Should_Set_Status_And_Result()
    {
        var a = new AnalysisRecord("测试");
        Assert.Equal("Pending", a.Status);
        a.Complete("OK");
        Assert.Equal("Completed", a.Status);
        Assert.Equal("OK", a.Result);
        Assert.True(a.CompletedAt.HasValue);
    }
}
