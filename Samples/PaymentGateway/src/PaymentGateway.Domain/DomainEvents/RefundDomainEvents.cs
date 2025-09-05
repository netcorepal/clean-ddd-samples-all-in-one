using PaymentGateway.Domain.AggregatesModel.RefundAggregate;

namespace PaymentGateway.Domain.DomainEvents;

public record RefundRequestedDomainEvent(Refund Refund) : IDomainEvent;

public record RefundSucceededDomainEvent(Refund Refund) : IDomainEvent;

public record RefundFailedDomainEvent(Refund Refund, string Reason) : IDomainEvent;
