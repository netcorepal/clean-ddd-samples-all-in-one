using PaymentGateway.Domain.AggregatesModel.OrderAggregate;

namespace PaymentGateway.Web.Application.IntegrationEventHandlers
{
    public record OrderPaidIntegrationEvent(OrderId OrderId);
}
