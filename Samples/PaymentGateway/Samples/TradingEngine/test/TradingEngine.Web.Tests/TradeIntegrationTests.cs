using TradingEngine.Domain.AggregatesModel.TradeAggregate;
using TradingEngine.Web.Application.Commands.Trade;
using TradingEngine.Web.Application.Queries.Trade;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace TradingEngine.Web.Tests;

public class TradeIntegrationTests : IClassFixture<MyWebApplicationFactory>
{
    private readonly MyWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public TradeIntegrationTests(MyWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateTrade_ShouldReturnTradeId()
    {
        // Arrange
        var command = new CreateTradeCommand("AAPL", TradeType.Buy, 100m, 150.50m, "testuser");

        // Act
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var result = await mediator.Send(command);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateAndRetrieveTrade_ShouldReturnCorrectData()
    {
        // Arrange
        var createCommand = new CreateTradeCommand("MSFT", TradeType.Sell, 50m, 200.00m, "testuser");

        // Act
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var tradeId = await mediator.Send(createCommand);

        var getQuery = new GetTradeQuery(tradeId);
        var trade = await mediator.Send(getQuery);

        // Assert
        Assert.NotNull(trade);
        Assert.Equal("MSFT", trade.Symbol);
        Assert.Equal(TradeType.Sell, trade.TradeType);
        Assert.Equal(50m, trade.Quantity);
        Assert.Equal(200.00m, trade.Price);
        Assert.Equal(TradeStatus.Pending, trade.Status);
    }

    [Fact]
    public async Task ExecuteTrade_ShouldUpdateTradeStatus()
    {
        // Arrange
        var createCommand = new CreateTradeCommand("GOOGL", TradeType.Buy, 25m, 2500.00m, "testuser");
        
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var tradeId = await mediator.Send(createCommand);

        var executeCommand = new ExecuteTradeCommand(tradeId, 25m, 2500.00m);

        // Act
        await mediator.Send(executeCommand);

        // Verify
        var getQuery = new GetTradeQuery(tradeId);
        var trade = await mediator.Send(getQuery);

        // Assert
        Assert.Equal(TradeStatus.Executed, trade.Status);
        Assert.Equal(25m, trade.ExecutedQuantity);
        Assert.NotNull(trade.ExecutedAt);
    }

    [Fact]
    public async Task CancelTrade_ShouldUpdateTradeStatus()
    {
        // Arrange
        var createCommand = new CreateTradeCommand("TSLA", TradeType.Buy, 10m, 800.00m, "testuser");
        
        using var scope = _factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var tradeId = await mediator.Send(createCommand);

        var cancelCommand = new CancelTradeCommand(tradeId);

        // Act
        await mediator.Send(cancelCommand);

        // Verify
        var getQuery = new GetTradeQuery(tradeId);
        var trade = await mediator.Send(getQuery);

        // Assert
        Assert.Equal(TradeStatus.Cancelled, trade.Status);
    }
}
