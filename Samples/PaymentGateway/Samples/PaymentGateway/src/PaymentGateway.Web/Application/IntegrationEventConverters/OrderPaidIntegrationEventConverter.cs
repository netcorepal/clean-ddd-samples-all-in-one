using PaymentGateway.Domain.DomainEvents;
using PaymentGateway.Web.Application.IntegrationEventHandlers;
using NetCorePal.Extensions.DistributedTransactions;

namespace PaymentGateway.Web.Application.IntegrationEventConverters;

public class OrderPaidIntegrationEventConverter
    : IIntegrationEventConverter<OrderPaidDomainEvent, OrderPaidIntegrationEvent>
{
    public OrderPaidIntegrationEvent Convert(OrderPaidDomainEvent domainEvent)
    {
        return new OrderPaidIntegrationEvent(domainEvent.Order.Id);
    }
}