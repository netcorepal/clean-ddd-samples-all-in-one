using PaymentGateway.Domain.DomainEvents;
using PaymentGateway.Infrastructure.Repositories;

namespace PaymentGateway.Web.Application.DomainEventHandlers;

public class PaymentSucceededDomainEventHandlerForMarkOrderPaid(IOrderRepository orderRepository)
    : IDomainEventHandler<PaymentSucceededDomainEvent>
{
    public async Task Handle(PaymentSucceededDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var payment = domainEvent.Payment;
        var order = await orderRepository.GetAsync(payment.OrderId, cancellationToken)
                   ?? throw new KnownException($"Order not found, OrderId = {payment.OrderId}");

        if (!order.Paid)
        {
            order.OrderPaid();
            await orderRepository.UpdateAsync(order, cancellationToken);
        }
    }
}
