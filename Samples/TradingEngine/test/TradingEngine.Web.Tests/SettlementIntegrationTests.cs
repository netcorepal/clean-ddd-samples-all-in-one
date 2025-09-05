using TradingEngine.Domain.AggregatesModel.SettlementAggregate;
using TradingEngine.Web.Application.Commands.Settlement;
using TradingEngine.Web.Application.Queries.Settlement;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace TradingEngine.Web.Tests;

public class SettlementIntegrationTests : IClassFixture<MyWebApplicationFactory>
{
    private readonly MyWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public SettlementIntegrationTests(MyWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateSettlement_ShouldReturnSettlementId()
    {
        // Arrange
        var command = new CreateSettlementCommand("testuser", SettlementType.TradeSettlement, DateTimeOffset.UtcNow.AddDays(1));

        // Act
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var result = await mediator.Send(command);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateAndRetrieveSettlement_ShouldReturnCorrectData()
    {
        // Arrange
        var settlementDate = DateTimeOffset.UtcNow.AddDays(2);
        var createCommand = new CreateSettlementCommand("testuser2", SettlementType.DividendSettlement, settlementDate);

        // Act
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var settlementId = await mediator.Send(createCommand);

        var getQuery = new GetSettlementDetailQuery(settlementId);
        var settlement = await mediator.Send(getQuery);

        // Assert
        Assert.NotNull(settlement);
        Assert.Equal("testuser2", settlement.UserId);
        Assert.Equal(SettlementType.DividendSettlement, settlement.SettlementType);
        Assert.Equal(SettlementStatus.Pending, settlement.Status);
    }

    [Fact]
    public async Task ProcessSettlement_ShouldUpdateStatus()
    {
        // Arrange
        var createCommand = new CreateSettlementCommand("testuser3", SettlementType.FeeSettlement, DateTimeOffset.UtcNow.AddDays(1));
        
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var settlementId = await mediator.Send(createCommand);

        var processCommand = new ProcessSettlementCommand(settlementId);

        // Act
        await mediator.Send(processCommand);

        // Verify
        var getQuery = new GetSettlementDetailQuery(settlementId);
        var settlement = await mediator.Send(getQuery);

        // Assert
        Assert.Equal(SettlementStatus.Processing, settlement.Status);
        Assert.NotNull(settlement.ProcessedAt);
    }
}
