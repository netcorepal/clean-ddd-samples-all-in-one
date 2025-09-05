using TradingEngine.Domain.AggregatesModel.TradeAggregate;

namespace TradingEngine.Domain.DomainEvents;

public record TradeCreatedDomainEvent(Trade Trade) : IDomainEvent;

public record TradeExecutedDomainEvent(Trade Trade) : IDomainEvent;

public record TradePartiallyFilledDomainEvent(Trade Trade, decimal ExecutedQuantity, decimal ExecutedPrice) : IDomainEvent;

public record TradeFailedDomainEvent(Trade Trade, string Reason) : IDomainEvent;

public record TradeCancelledDomainEvent(Trade Trade) : IDomainEvent;
