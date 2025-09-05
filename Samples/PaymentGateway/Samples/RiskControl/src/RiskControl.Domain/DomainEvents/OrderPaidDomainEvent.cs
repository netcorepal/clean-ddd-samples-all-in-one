using RiskControl.Domain.AggregatesModel.OrderAggregate;

namespace RiskControl.Domain.DomainEvents;

public record OrderPaidDomainEvent(Order Order) : IDomainEvent;