using RiskControl.Domain.DomainEvents;

namespace RiskControl.Web.Application.DomainEventHandlers;

public class FraudCheckCompletedDomainEventHandler(ILogger<FraudCheckCompletedDomainEventHandler> logger) : INotificationHandler<FraudCheckCompletedDomainEvent>
{
    public Task Handle(FraudCheckCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fraud check completed for order {OrderId} with result {Result} and score {Score}", notification.FraudCheck.OrderId, notification.FraudCheck.Result, notification.FraudCheck.RiskScore);
        return Task.CompletedTask;
    }
}

public class CreditAssessmentCompletedDomainEventHandler(ILogger<CreditAssessmentCompletedDomainEventHandler> logger) : INotificationHandler<CreditAssessmentCompletedDomainEvent>
{
    public Task Handle(CreditAssessmentCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Credit assessment completed for order {OrderId} with grade {Grade} and score {Score}", notification.CreditAssessment.OrderId, notification.CreditAssessment.Grade, notification.CreditAssessment.Score);
        return Task.CompletedTask;
    }
}

public class ComplianceAlertClosedDomainEventHandler(ILogger<ComplianceAlertClosedDomainEventHandler> logger) : INotificationHandler<ComplianceAlertClosedDomainEvent>
{
    public Task Handle(ComplianceAlertClosedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Compliance alert closed for order {OrderId} with resolution {Resolution}", notification.ComplianceAlert.OrderId, notification.ComplianceAlert.Resolution);
        return Task.CompletedTask;
    }
}
