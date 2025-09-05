using ReportingService.Domain.AggregatesModel.OrderAggregate;

namespace ReportingService.Domain.DomainEvents;

public record OrderPaidDomainEvent(Order Order) : IDomainEvent;