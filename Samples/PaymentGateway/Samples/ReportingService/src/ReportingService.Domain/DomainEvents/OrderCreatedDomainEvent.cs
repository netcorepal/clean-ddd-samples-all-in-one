using ReportingService.Domain.AggregatesModel.OrderAggregate;

namespace ReportingService.Domain.DomainEvents
{
    public record OrderCreatedDomainEvent(Order Order) : IDomainEvent;
}
