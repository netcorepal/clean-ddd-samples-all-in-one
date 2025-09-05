using ReportingService.Domain.AggregatesModel.RegulatoryReportAggregate;

namespace ReportingService.Domain.DomainEvents;

public record RegulatoryReportCreatedDomainEvent(RegulatoryReport Report) : IDomainEvent;
