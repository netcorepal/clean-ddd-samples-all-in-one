using TradingEngine.Domain.AggregatesModel.OrderAggregate;

namespace TradingEngine.Domain.DomainEvents;

public record OrderPaidDomainEvent(Order Order) : IDomainEvent;