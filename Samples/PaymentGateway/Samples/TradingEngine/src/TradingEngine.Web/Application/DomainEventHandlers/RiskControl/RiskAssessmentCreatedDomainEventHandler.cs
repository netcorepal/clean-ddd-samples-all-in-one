using TradingEngine.Domain.DomainEvents;

namespace TradingEngine.Web.Application.DomainEventHandlers.RiskControl;

public class RiskAssessmentCreatedDomainEventHandler : INotificationHandler<RiskAssessmentCreatedDomainEvent>
{
    private readonly ILogger<RiskAssessmentCreatedDomainEventHandler> _logger;

    public RiskAssessmentCreatedDomainEventHandler(ILogger<RiskAssessmentCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(RiskAssessmentCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var riskControl = notification.RiskControl;
        var assessment = notification.Assessment;
        
        _logger.LogInformation("风险评估完成: UserId={UserId}, Symbol={Symbol}, RiskLevel={RiskLevel}, Description={Description}", 
            riskControl.UserId, assessment.Symbol, assessment.RiskLevel, assessment.Description);

        // 高风险交易记录特殊日志
        if (assessment.RiskLevel >= Domain.AggregatesModel.RiskControlAggregate.RiskLevel.High)
        {
            _logger.LogWarning("高风险交易: UserId={UserId}, Symbol={Symbol}, RiskLevel={RiskLevel}, RiskTypes={RiskTypes}", 
                riskControl.UserId, assessment.Symbol, assessment.RiskLevel, string.Join(",", assessment.RiskTypes));
        }

        await Task.CompletedTask;
    }
}
