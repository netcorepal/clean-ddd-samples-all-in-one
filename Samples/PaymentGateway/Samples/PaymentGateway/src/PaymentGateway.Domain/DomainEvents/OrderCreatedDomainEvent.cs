using PaymentGateway.Domain.AggregatesModel.OrderAggregate;

namespace PaymentGateway.Domain.DomainEvents
{
    public record OrderCreatedDomainEvent(Order Order) : IDomainEvent;
}
