using TradingEngine.Domain.AggregatesModel.TradeAggregate;

namespace TradingEngine.Domain.Tests;

public class TradeTests
{
    [Fact]
    public void CreateTrade_ShouldInitializeCorrectly()
    {
        // Arrange
        var symbol = "AAPL";
        var tradeType = TradeType.Buy;
        var quantity = 100m;
        var price = 150.50m;
        var userId = "user123";

        // Act
        var trade = new Trade(symbol, tradeType, quantity, price, userId);

        // Assert
        Assert.Equal(symbol, trade.Symbol);
        Assert.Equal(tradeType, trade.TradeType);
        Assert.Equal(quantity, trade.Quantity);
        Assert.Equal(price, trade.Price);
        Assert.Equal(userId, trade.UserId);
        Assert.Equal(TradeStatus.Pending, trade.Status);
        Assert.Equal(0, trade.ExecutedQuantity);
        Assert.Equal(quantity, trade.RemainingQuantity);
        Assert.Equal(quantity * price, trade.TotalValue);
    }

    [Fact]
    public void ExecuteTrade_FullyExecuted_ShouldUpdateStatusToExecuted()
    {
        // Arrange
        var trade = new Trade("AAPL", TradeType.Buy, 100m, 150.50m, "user123");
        var executedQuantity = 100m;
        var executedPrice = 150.50m;

        // Act
        trade.Execute(executedQuantity, executedPrice);

        // Assert
        Assert.Equal(TradeStatus.Executed, trade.Status);
        Assert.Equal(executedQuantity, trade.ExecutedQuantity);
        Assert.Equal(0, trade.RemainingQuantity);
        Assert.NotNull(trade.ExecutedAt);
    }

    [Fact]
    public void ExecuteTrade_PartiallyExecuted_ShouldUpdateStatusToPartiallyFilled()
    {
        // Arrange
        var trade = new Trade("AAPL", TradeType.Buy, 100m, 150.50m, "user123");
        var executedQuantity = 50m;
        var executedPrice = 150.50m;

        // Act
        trade.Execute(executedQuantity, executedPrice);

        // Assert
        Assert.Equal(TradeStatus.PartiallyFilled, trade.Status);
        Assert.Equal(executedQuantity, trade.ExecutedQuantity);
        Assert.Equal(50m, trade.RemainingQuantity);
        Assert.Null(trade.ExecutedAt);
    }

    [Fact]
    public void ExecuteTrade_ExceedsRemainingQuantity_ShouldThrowException()
    {
        // Arrange
        var trade = new Trade("AAPL", TradeType.Buy, 100m, 150.50m, "user123");

        // Act & Assert
        Assert.Throws<KnownException>(() => trade.Execute(150m, 150.50m));
    }

    [Fact]
    public void CancelTrade_PendingStatus_ShouldUpdateStatusToCancelled()
    {
        // Arrange
        var trade = new Trade("AAPL", TradeType.Buy, 100m, 150.50m, "user123");

        // Act
        trade.Cancel();

        // Assert
        Assert.Equal(TradeStatus.Cancelled, trade.Status);
    }

    [Fact]
    public void CancelTrade_ExecutedStatus_ShouldThrowException()
    {
        // Arrange
        var trade = new Trade("AAPL", TradeType.Buy, 100m, 150.50m, "user123");
        trade.Execute(100m, 150.50m);

        // Act & Assert
        Assert.Throws<KnownException>(() => trade.Cancel());
    }

    [Fact]
    public void FailTrade_PendingStatus_ShouldUpdateStatusToFailed()
    {
        // Arrange
        var trade = new Trade("AAPL", TradeType.Buy, 100m, 150.50m, "user123");
        var reason = "Insufficient funds";

        // Act
        trade.Fail(reason);

        // Assert
        Assert.Equal(TradeStatus.Failed, trade.Status);
        Assert.Equal(reason, trade.FailureReason);
    }

    [Fact]
    public void ExecuteTrade_AlreadyExecuted_ShouldThrowException()
    {
        // Arrange
        var trade = new Trade("AAPL", TradeType.Buy, 100m, 150.50m, "user123");
        trade.Execute(100m, 150.50m);

        // Act & Assert
        Assert.Throws<KnownException>(() => trade.Execute(50m, 150.50m));
    }
}
