using ReportingService.Domain.AggregatesModel.OrderAggregate;

namespace ReportingService.Web.Application.IntegrationEventHandlers
{
    public record OrderPaidIntegrationEvent(OrderId OrderId);
}
