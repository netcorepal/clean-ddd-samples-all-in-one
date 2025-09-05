using RiskControl.Domain.AggregatesModel.OrderAggregate;

namespace RiskControl.Domain.DomainEvents
{
    public record OrderCreatedDomainEvent(Order Order) : IDomainEvent;
}
