using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;

namespace PaymentGateway.Domain.DomainEvents;

public record PaymentInitiatedDomainEvent(Payment Payment) : IDomainEvent;

public record PaymentSucceededDomainEvent(Payment Payment) : IDomainEvent;

public record PaymentFailedDomainEvent(Payment Payment, string Reason) : IDomainEvent;
