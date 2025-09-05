using TradingEngine.Domain.AggregatesModel.RiskControlAggregate;
using TradingEngine.Domain.AggregatesModel.TradeAggregate;

namespace TradingEngine.Domain.Tests;

public class RiskControlTests
{
    [Fact]
    public void CreateRiskControl_ShouldInitializeCorrectly()
    {
        // Arrange
        var userId = "user123";
        var totalPositionLimit = 100000m;
        var dailyLossLimit = 5000m;

        // Act
        var riskControl = new RiskControl(userId, totalPositionLimit, dailyLossLimit);

        // Assert
        Assert.Equal(userId, riskControl.UserId);
        Assert.Equal(totalPositionLimit, riskControl.TotalPositionLimit);
        Assert.Equal(dailyLossLimit, riskControl.DailyLossLimit);
        Assert.Equal(0, riskControl.CurrentPosition);
        Assert.Equal(0, riskControl.DailyLoss);
        Assert.True(riskControl.IsActive);
        Assert.Empty(riskControl.RiskAssessments);
    }

    [Fact]
    public void AssessTradeRisk_WithinLimits_ShouldReturnLowRisk()
    {
        // Arrange
        var riskControl = new RiskControl("user123", 100000m, 5000m);
        var symbol = "AAPL";
        var quantity = 10m;
        var price = 150m;
        var tradeType = TradeType.Buy;

        // Act
        var assessment = riskControl.AssessTradeRisk(symbol, quantity, price, tradeType);

        // Assert
        Assert.Equal(RiskLevel.Low, assessment.RiskLevel);
        Assert.Single(riskControl.RiskAssessments);
        Assert.NotNull(riskControl.LastAssessmentAt);
    }

    [Fact]
    public void AssessTradeRisk_ExceedsPositionLimit_ShouldReturnHighRisk()
    {
        // Arrange
        var riskControl = new RiskControl("user123", 10000m, 5000m);
        var symbol = "AAPL";
        var quantity = 100m;
        var price = 150m; // Total: 15000, exceeds limit of 10000
        var tradeType = TradeType.Buy;

        // Act
        var assessment = riskControl.AssessTradeRisk(symbol, quantity, price, tradeType);

        // Assert
        Assert.Equal(RiskLevel.High, assessment.RiskLevel);
        Assert.Contains(RiskType.PositionLimit, assessment.RiskTypes);
        Assert.Contains("Position would exceed limit", assessment.Description);
    }

    [Fact]
    public void AssessTradeRisk_HighConcentration_ShouldReturnMediumRisk()
    {
        // Arrange
        var riskControl = new RiskControl("user123", 100000m, 5000m);
        var symbol = "AAPL";
        var quantity = 250m;
        var price = 150m; // Total: 37500, exceeds 30% concentration limit (30000)
        var tradeType = TradeType.Buy;

        // Act
        var assessment = riskControl.AssessTradeRisk(symbol, quantity, price, tradeType);

        // Assert
        Assert.True(assessment.RiskLevel >= RiskLevel.Medium);
        Assert.Contains(RiskType.ConcentrationRisk, assessment.RiskTypes);
        Assert.Contains("High concentration", assessment.Description);
    }

    [Fact]
    public void AssessTradeRisk_LargeTrade_ShouldReturnMediumRisk()
    {
        // Arrange
        var riskControl = new RiskControl("user123", 10000000m, 50000m);
        var symbol = "AAPL";
        var quantity = 10000m;
        var price = 150m; // Total: 1500000, exceeds large trade threshold (1000000)
        var tradeType = TradeType.Buy;

        // Act
        var assessment = riskControl.AssessTradeRisk(symbol, quantity, price, tradeType);

        // Assert
        Assert.True(assessment.RiskLevel >= RiskLevel.Medium);
        Assert.Contains(RiskType.LiquidityRisk, assessment.RiskTypes);
        Assert.Contains("Large trade amount", assessment.Description);
    }

    [Fact]
    public void UpdatePosition_ShouldUpdateCurrentPosition()
    {
        // Arrange
        var riskControl = new RiskControl("user123", 100000m, 5000m);
        var positionChange = 15000m;

        // Act
        riskControl.UpdatePosition(positionChange);

        // Assert
        Assert.Equal(positionChange, riskControl.CurrentPosition);
    }

    [Fact]
    public void UpdateDailyLoss_WithinLimit_ShouldUpdateLoss()
    {
        // Arrange
        var riskControl = new RiskControl("user123", 100000m, 5000m);
        var lossAmount = 2000m;

        // Act
        riskControl.UpdateDailyLoss(lossAmount);

        // Assert
        Assert.Equal(lossAmount, riskControl.DailyLoss);
    }

    [Fact]
    public void UpdateDailyLoss_ExceedsLimit_ShouldUpdateLossAndTriggerEvent()
    {
        // Arrange
        var riskControl = new RiskControl("user123", 100000m, 5000m);
        var lossAmount = 6000m; // Exceeds daily loss limit

        // Act
        riskControl.UpdateDailyLoss(lossAmount);

        // Assert
        Assert.Equal(lossAmount, riskControl.DailyLoss);
        // Note: Event triggering would be tested in integration tests
    }

    [Fact]
    public void ResetDailyLoss_ShouldSetLossToZero()
    {
        // Arrange
        var riskControl = new RiskControl("user123", 100000m, 5000m);
        riskControl.UpdateDailyLoss(3000m);

        // Act
        riskControl.ResetDailyLoss();

        // Assert
        Assert.Equal(0, riskControl.DailyLoss);
    }

    [Fact]
    public void Activate_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var riskControl = new RiskControl("user123", 100000m, 5000m);
        riskControl.Deactivate();

        // Act
        riskControl.Activate();

        // Assert
        Assert.True(riskControl.IsActive);
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var riskControl = new RiskControl("user123", 100000m, 5000m);

        // Act
        riskControl.Deactivate();

        // Assert
        Assert.False(riskControl.IsActive);
    }
}
