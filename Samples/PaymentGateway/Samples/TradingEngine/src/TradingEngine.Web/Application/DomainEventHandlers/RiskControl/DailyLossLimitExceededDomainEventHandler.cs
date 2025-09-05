using TradingEngine.Domain.DomainEvents;

namespace TradingEngine.Web.Application.DomainEventHandlers.RiskControl;

public class DailyLossLimitExceededDomainEventHandler : INotificationHandler<DailyLossLimitExceededDomainEvent>
{
    private readonly ILogger<DailyLossLimitExceededDomainEventHandler> _logger;

    public DailyLossLimitExceededDomainEventHandler(ILogger<DailyLossLimitExceededDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(DailyLossLimitExceededDomainEvent notification, CancellationToken cancellationToken)
    {
        var riskControl = notification.RiskControl;
        
        _logger.LogWarning("用户日损失超限: UserId={UserId}, CurrentLoss={CurrentLoss}, Limit={Limit}", 
            riskControl.UserId, notification.CurrentLoss, notification.Limit);

        // 这里可以添加风险处理逻辑，比如：
        // - 发送警报通知
        // - 自动停止交易
        // - 强制平仓
        // - 通知风险管理人员

        await Task.CompletedTask;
    }
}
