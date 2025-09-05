using RiskControl.Domain.AggregatesModel.OrderAggregate;

namespace RiskControl.Web.Application.IntegrationEventHandlers
{
    public record OrderPaidIntegrationEvent(OrderId OrderId);
}
