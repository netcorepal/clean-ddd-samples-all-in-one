using ReportingService.Domain.AggregatesModel.FinancialReportAggregate;

namespace ReportingService.Domain.DomainEvents;

public record FinancialReportCreatedDomainEvent(FinancialReport Report) : IDomainEvent;
