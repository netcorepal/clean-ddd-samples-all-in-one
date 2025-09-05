using TradingEngine.Domain.DomainEvents;

namespace TradingEngine.Web.Application.DomainEventHandlers.Trade;

public class TradeCreatedDomainEventHandler : INotificationHandler<TradeCreatedDomainEvent>
{
    private readonly ILogger<TradeCreatedDomainEventHandler> _logger;

    public TradeCreatedDomainEventHandler(ILogger<TradeCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(TradeCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var trade = notification.Trade;
        
        _logger.LogInformation("交易创建成功: TradeId={TradeId}, Symbol={Symbol}, Type={TradeType}, Quantity={Quantity}, Price={Price}", 
            trade.Id, trade.Symbol, trade.TradeType, trade.Quantity, trade.Price);

        // 这里可以添加其他业务逻辑，比如：
        // - 发送通知
        // - 记录审计日志
        // - 触发其他业务流程

        await Task.CompletedTask;
    }
}
