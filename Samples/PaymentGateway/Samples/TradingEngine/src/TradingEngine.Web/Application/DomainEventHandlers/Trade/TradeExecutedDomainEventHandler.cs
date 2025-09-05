using TradingEngine.Domain.DomainEvents;
using TradingEngine.Domain.AggregatesModel.SettlementAggregate;
using TradingEngine.Infrastructure.Repositories;

namespace TradingEngine.Web.Application.DomainEventHandlers.Trade;

public class TradeExecutedDomainEventHandler : INotificationHandler<TradeExecutedDomainEvent>
{
    private readonly ISettlementRepository _settlementRepository;
    private readonly IRiskControlRepository _riskControlRepository;
    private readonly ILogger<TradeExecutedDomainEventHandler> _logger;

    public TradeExecutedDomainEventHandler(
        ISettlementRepository settlementRepository,
        IRiskControlRepository riskControlRepository,
        ILogger<TradeExecutedDomainEventHandler> logger)
    {
        _settlementRepository = settlementRepository;
        _riskControlRepository = riskControlRepository;
        _logger = logger;
    }

    public async Task Handle(TradeExecutedDomainEvent notification, CancellationToken cancellationToken)
    {
        var trade = notification.Trade;
        
        _logger.LogInformation("处理交易执行事件: TradeId={TradeId}, Symbol={Symbol}, Quantity={Quantity}", 
            trade.Id, trade.Symbol, trade.ExecutedQuantity);

        try
        {
            // 1. 创建结算记录
            await CreateSettlement(trade, cancellationToken);

            // 2. 更新风险控制持仓
            await UpdateRiskControlPosition(trade, cancellationToken);

            _logger.LogInformation("交易执行事件处理完成: TradeId={TradeId}", trade.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "处理交易执行事件时发生错误: TradeId={TradeId}", trade.Id);
            throw;
        }
    }

    private async Task CreateSettlement(Domain.AggregatesModel.TradeAggregate.Trade trade, CancellationToken cancellationToken)
    {
        var settlement = new Domain.AggregatesModel.SettlementAggregate.Settlement(
            trade.UserId,
            SettlementType.TradeSettlement,
            0,
            DateTimeOffset.UtcNow.Date.AddDays(1)); // T+1结算

        settlement.AddTradeSettlementItem(
            trade.Id,
            trade.Symbol,
            trade.ExecutedQuantity,
            trade.Price,
            trade.TradeType);

        // 添加交易费用（假设为成交金额的0.1%）
        var fee = trade.ExecutedQuantity * trade.Price * 0.001m;
        settlement.AddFeeSettlementItem("交易费用", fee);

        await _settlementRepository.AddAsync(settlement, cancellationToken);
    }

    private async Task UpdateRiskControlPosition(Domain.AggregatesModel.TradeAggregate.Trade trade, CancellationToken cancellationToken)
    {
        var riskControl = await _riskControlRepository.GetByUserIdAsync(trade.UserId, cancellationToken);
        if (riskControl != null)
        {
            var positionChange = trade.TradeType == Domain.AggregatesModel.TradeAggregate.TradeType.Buy 
                ? trade.ExecutedQuantity * trade.Price 
                : -(trade.ExecutedQuantity * trade.Price);

            riskControl.UpdatePosition(positionChange);
            await _riskControlRepository.UpdateAsync(riskControl, cancellationToken);
        }
    }
}
