using FinancialServices.Backend.Domain.AggregatesModel.OrderAggregate;

namespace FinancialServices.Backend.Web.Application.IntegrationEventHandlers
{
    public record OrderPaidIntegrationEvent(OrderId OrderId);
}
