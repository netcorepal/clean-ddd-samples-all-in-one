using TradingEngine.Domain.AggregatesModel.OrderAggregate;

namespace TradingEngine.Domain.DomainEvents
{
    public record OrderCreatedDomainEvent(Order Order) : IDomainEvent;
}
