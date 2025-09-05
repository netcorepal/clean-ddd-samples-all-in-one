using TradingEngine.Domain.AggregatesModel.RiskControlAggregate;
using TradingEngine.Web.Application.Commands.RiskControl;
using TradingEngine.Web.Application.Queries.RiskControl;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace TradingEngine.Web.Tests;

public class RiskControlIntegrationTests : IClassFixture<MyWebApplicationFactory>
{
    private readonly MyWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public RiskControlIntegrationTests(MyWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateRiskControl_ShouldReturnRiskControlId()
    {
        // Arrange
        var command = new CreateRiskControlCommand("testuser", 10000m, 5000m);

        // Act
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var result = await mediator.Send(command);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateAndRetrieveRiskControl_ShouldReturnCorrectData()
    {
        // Arrange
        var userId = $"testuser_{Guid.NewGuid()}";
        var createCommand = new CreateRiskControlCommand(userId, 20000m, 8000m);

        // Act
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var riskControlId = await mediator.Send(createCommand);

        var getQuery = new GetRiskControlByUserQuery(userId);
        var riskControl = await mediator.Send(getQuery);

        // Assert
        Assert.NotNull(riskControl);
        Assert.Equal(userId, riskControl.UserId);
        Assert.Equal(20000m, riskControl.TotalPositionLimit);
        Assert.Equal(8000m, riskControl.DailyLossLimit);
        Assert.True(riskControl.IsActive);
    }

    [Fact]
    public async Task UpdatePosition_ShouldUpdatePositionCorrectly()
    {
        // Arrange
        var userId = $"testuser_{Guid.NewGuid()}";
        var createCommand = new CreateRiskControlCommand(userId, 15000m, 6000m);
        
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var riskControlId = await mediator.Send(createCommand);

        var updateCommand = new UpdatePositionCommand(userId, 5000m);

        // Act
        await mediator.Send(updateCommand);

        // Verify
        var getQuery = new GetRiskControlByUserQuery(userId);
        var riskControl = await mediator.Send(getQuery);

        // Assert
        Assert.Equal(5000m, riskControl.CurrentPosition);
    }
}
