using FinancialServices.Backend.Domain.AggregatesModel.OrderAggregate;

namespace FinancialServices.Backend.Domain.DomainEvents;

public record OrderPaidDomainEvent(Order Order) : IDomainEvent;