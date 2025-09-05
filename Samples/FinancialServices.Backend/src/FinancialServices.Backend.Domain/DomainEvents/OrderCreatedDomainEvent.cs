using FinancialServices.Backend.Domain.AggregatesModel.OrderAggregate;

namespace FinancialServices.Backend.Domain.DomainEvents
{
    public record OrderCreatedDomainEvent(Order Order) : IDomainEvent;
}
