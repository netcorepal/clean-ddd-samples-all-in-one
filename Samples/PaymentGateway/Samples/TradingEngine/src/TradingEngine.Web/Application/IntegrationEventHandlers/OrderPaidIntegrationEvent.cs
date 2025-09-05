using TradingEngine.Domain.AggregatesModel.OrderAggregate;

namespace TradingEngine.Web.Application.IntegrationEventHandlers
{
    public record OrderPaidIntegrationEvent(OrderId OrderId);
}
