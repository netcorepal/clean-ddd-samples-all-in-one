using TradingEngine.Domain.AggregatesModel.SettlementAggregate;

namespace TradingEngine.Domain.DomainEvents;

public record SettlementCreatedDomainEvent(Settlement Settlement) : IDomainEvent;

public record SettlementItemAddedDomainEvent(Settlement Settlement, SettlementItem Item) : IDomainEvent;

public record SettlementProcessingStartedDomainEvent(Settlement Settlement) : IDomainEvent;

public record SettlementCompletedDomainEvent(Settlement Settlement) : IDomainEvent;

public record SettlementFailedDomainEvent(Settlement Settlement, string Reason) : IDomainEvent;

public record SettlementCancelledDomainEvent(Settlement Settlement) : IDomainEvent;
