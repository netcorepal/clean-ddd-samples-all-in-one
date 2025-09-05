using TradingEngine.Domain.AggregatesModel.SettlementAggregate;
using TradingEngine.Domain.AggregatesModel.TradeAggregate;

namespace TradingEngine.Domain.Tests;

public class SettlementTests
{
    [Fact]
    public void CreateSettlement_ShouldInitializeCorrectly()
    {
        // Arrange
        var userId = "user123";
        var settlementType = SettlementType.TradeSettlement;
        var totalAmount = 0m;
        var settlementDate = DateTimeOffset.UtcNow.Date.AddDays(1);

        // Act
        var settlement = new Settlement(userId, settlementType, totalAmount, settlementDate);

        // Assert
        Assert.Equal(userId, settlement.UserId);
        Assert.Equal(settlementType, settlement.SettlementType);
        Assert.Equal(totalAmount, settlement.TotalAmount);
        Assert.Equal(settlementDate, settlement.SettlementDate);
        Assert.Equal(SettlementStatus.Pending, settlement.Status);
        Assert.Empty(settlement.Items);
    }

    [Fact]
    public void AddTradeSettlementItem_BuyTrade_ShouldAddNegativeAmount()
    {
        // Arrange
        var settlement = new Settlement("user123", SettlementType.TradeSettlement, 0m, DateTimeOffset.UtcNow.Date.AddDays(1));
        var tradeId = new TradeId(Guid.NewGuid());
        var symbol = "AAPL";
        var quantity = 100m;
        var price = 150m;
        var tradeType = TradeType.Buy;

        // Act
        settlement.AddTradeSettlementItem(tradeId, symbol, quantity, price, tradeType);

        // Assert
        Assert.Single(settlement.Items);
        Assert.Equal(-15000m, settlement.TotalAmount); // Buy = negative amount
        var item = settlement.Items.First();
        Assert.Equal(tradeId.ToString(), item.ReferenceId);
        Assert.Equal(symbol, item.Symbol);
        Assert.Equal(quantity, item.Quantity);
        Assert.Equal(price, item.Price);
        Assert.Equal(-15000m, item.Amount);
    }

    [Fact]
    public void AddTradeSettlementItem_SellTrade_ShouldAddPositiveAmount()
    {
        // Arrange
        var settlement = new Settlement("user123", SettlementType.TradeSettlement, 0m, DateTimeOffset.UtcNow.Date.AddDays(1));
        var tradeId = new TradeId(Guid.NewGuid());
        var symbol = "AAPL";
        var quantity = 100m;
        var price = 150m;
        var tradeType = TradeType.Sell;

        // Act
        settlement.AddTradeSettlementItem(tradeId, symbol, quantity, price, tradeType);

        // Assert
        Assert.Single(settlement.Items);
        Assert.Equal(15000m, settlement.TotalAmount); // Sell = positive amount
        var item = settlement.Items.First();
        Assert.Equal(15000m, item.Amount);
    }

    [Fact]
    public void AddFeeSettlementItem_ShouldAddNegativeFeeAmount()
    {
        // Arrange
        var settlement = new Settlement("user123", SettlementType.TradeSettlement, 0m, DateTimeOffset.UtcNow.Date.AddDays(1));
        var description = "Trading Fee";
        var feeAmount = 15m;

        // Act
        settlement.AddFeeSettlementItem(description, feeAmount);

        // Assert
        Assert.Single(settlement.Items);
        Assert.Equal(-15m, settlement.TotalAmount); // Fee = negative amount
        var item = settlement.Items.First();
        Assert.Equal("FEE", item.Symbol);
        Assert.Equal(1m, item.Quantity);
        Assert.Equal(feeAmount, item.Price);
        Assert.Equal(-feeAmount, item.Amount);
        Assert.Equal(description, item.Description);
    }

    [Fact]
    public void AddMultipleItems_ShouldRecalculateTotalAmount()
    {
        // Arrange
        var settlement = new Settlement("user123", SettlementType.TradeSettlement, 0m, DateTimeOffset.UtcNow.Date.AddDays(1));
        var tradeId = new TradeId(Guid.NewGuid());        // Act
        settlement.AddTradeSettlementItem(tradeId, "AAPL", 100m, 150m, TradeType.Sell); // +15000
        settlement.AddFeeSettlementItem("Trading Fee", 15m); // -15
        settlement.AddFeeSettlementItem("Exchange Fee", 10m); // -10

        // Assert
        Assert.Equal(3, settlement.Items.Count);
        Assert.Equal(14975m, settlement.TotalAmount); // 15000 - 15 - 10
    }

    [Fact]
    public void StartProcessing_PendingStatus_ShouldUpdateToProcessing()
    {
        // Arrange
        var settlement = new Settlement("user123", SettlementType.TradeSettlement, 0m, DateTimeOffset.UtcNow.Date.AddDays(1));

        // Act
        settlement.StartProcessing();

        // Assert
        Assert.Equal(SettlementStatus.Processing, settlement.Status);
        Assert.NotNull(settlement.ProcessedAt);
    }

    [Fact]
    public void Complete_ProcessingStatus_ShouldUpdateToCompleted()
    {
        // Arrange
        var settlement = new Settlement("user123", SettlementType.TradeSettlement, 0m, DateTimeOffset.UtcNow.Date.AddDays(1));
        settlement.StartProcessing();

        // Act
        settlement.Complete();

        // Assert
        Assert.Equal(SettlementStatus.Completed, settlement.Status);
        Assert.NotNull(settlement.CompletedAt);
    }

    [Fact]
    public void Fail_ProcessingStatus_ShouldUpdateToFailed()
    {
        // Arrange
        var settlement = new Settlement("user123", SettlementType.TradeSettlement, 0m, DateTimeOffset.UtcNow.Date.AddDays(1));
        settlement.StartProcessing();
        var reason = "Insufficient funds";

        // Act
        settlement.Fail(reason);

        // Assert
        Assert.Equal(SettlementStatus.Failed, settlement.Status);
        Assert.Equal(reason, settlement.FailureReason);
    }

    [Fact]
    public void Cancel_PendingStatus_ShouldUpdateToCancelled()
    {
        // Arrange
        var settlement = new Settlement("user123", SettlementType.TradeSettlement, 0m, DateTimeOffset.UtcNow.Date.AddDays(1));

        // Act
        settlement.Cancel();

        // Assert
        Assert.Equal(SettlementStatus.Cancelled, settlement.Status);
    }

    [Fact]
    public void AddItemToNonPendingSettlement_ShouldThrowException()
    {
        // Arrange
        var settlement = new Settlement("user123", SettlementType.TradeSettlement, 0m, DateTimeOffset.UtcNow.Date.AddDays(1));
        settlement.StartProcessing();
        var tradeId = new TradeId(Guid.NewGuid());

        // Act & Assert
        Assert.Throws<KnownException>(() => 
            settlement.AddTradeSettlementItem(tradeId, "AAPL", 100m, 150m, TradeType.Buy));
    }

    [Fact]
    public void StartProcessingNonPendingSettlement_ShouldThrowException()
    {
        // Arrange
        var settlement = new Settlement("user123", SettlementType.TradeSettlement, 0m, DateTimeOffset.UtcNow.Date.AddDays(1));
        settlement.StartProcessing();

        // Act & Assert
        Assert.Throws<KnownException>(() => settlement.StartProcessing());
    }

    [Fact]
    public void CompleteNonProcessingSettlement_ShouldThrowException()
    {
        // Arrange
        var settlement = new Settlement("user123", SettlementType.TradeSettlement, 0m, DateTimeOffset.UtcNow.Date.AddDays(1));

        // Act & Assert
        Assert.Throws<KnownException>(() => settlement.Complete());
    }

    [Fact]
    public void CancelNonPendingSettlement_ShouldThrowException()
    {
        // Arrange
        var settlement = new Settlement("user123", SettlementType.TradeSettlement, 0m, DateTimeOffset.UtcNow.Date.AddDays(1));
        settlement.StartProcessing();

        // Act & Assert
        Assert.Throws<KnownException>(() => settlement.Cancel());
    }
}
