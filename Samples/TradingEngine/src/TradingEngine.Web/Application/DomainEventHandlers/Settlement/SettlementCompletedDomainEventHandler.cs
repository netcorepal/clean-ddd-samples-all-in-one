using TradingEngine.Domain.DomainEvents;

namespace TradingEngine.Web.Application.DomainEventHandlers.Settlement;

public class SettlementCompletedDomainEventHandler : INotificationHandler<SettlementCompletedDomainEvent>
{
    private readonly ILogger<SettlementCompletedDomainEventHandler> _logger;

    public SettlementCompletedDomainEventHandler(ILogger<SettlementCompletedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(SettlementCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var settlement = notification.Settlement;
        
        _logger.LogInformation("结算完成: SettlementId={SettlementId}, UserId={UserId}, TotalAmount={TotalAmount}", 
            settlement.Id, settlement.UserId, settlement.TotalAmount);

        // 这里可以添加结算完成后的业务逻辑，比如：
        // - 发送结算通知
        // - 更新账户余额
        // - 生成结算报告
        // - 同步到外部系统

        await Task.CompletedTask;
    }
}
