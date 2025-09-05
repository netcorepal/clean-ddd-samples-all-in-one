using TradingEngine.Domain.AggregatesModel.RiskControlAggregate;

namespace TradingEngine.Domain.DomainEvents;

public record RiskControlCreatedDomainEvent(RiskControl RiskControl) : IDomainEvent;

public record RiskAssessmentCreatedDomainEvent(RiskControl RiskControl, RiskAssessment Assessment) : IDomainEvent;

public record PositionUpdatedDomainEvent(RiskControl RiskControl, decimal PositionChange) : IDomainEvent;

public record DailyLossLimitExceededDomainEvent(RiskControl RiskControl, decimal CurrentLoss, decimal Limit) : IDomainEvent;

public record DailyLossResetDomainEvent(RiskControl RiskControl) : IDomainEvent;

public record RiskControlActivatedDomainEvent(RiskControl RiskControl) : IDomainEvent;

public record RiskControlDeactivatedDomainEvent(RiskControl RiskControl) : IDomainEvent;
