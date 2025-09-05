using PaymentGateway.Domain.AggregatesModel.OrderAggregate;

namespace PaymentGateway.Domain.DomainEvents;

public record OrderPaidDomainEvent(Order Order) : IDomainEvent;