using RiskControl.Domain.AggregatesModel.FraudAggregate;
using RiskControl.Domain.AggregatesModel.CreditAggregate;
using RiskControl.Domain.AggregatesModel.ComplianceAggregate;

namespace RiskControl.Domain.DomainEvents;

public record FraudCheckCompletedDomainEvent(FraudCheck FraudCheck) : IDomainEvent;

public record CreditAssessmentCompletedDomainEvent(CreditAssessment CreditAssessment) : IDomainEvent;

public record ComplianceAlertClosedDomainEvent(ComplianceAlert ComplianceAlert) : IDomainEvent;
