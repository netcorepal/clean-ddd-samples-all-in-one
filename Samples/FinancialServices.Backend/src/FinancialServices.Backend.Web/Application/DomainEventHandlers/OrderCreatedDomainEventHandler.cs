using FinancialServices.Backend.Domain.DomainEvents;
using FinancialServices.Backend.Web.Application.Commands;
using MediatR;
using NetCorePal.Extensions.Domain;

namespace FinancialServices.Backend.Web.Application.DomainEventHandlers
{
    internal class OrderCreatedDomainEventHandler(IMediator mediator) : IDomainEventHandler<OrderCreatedDomainEvent>
    {
        public Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            return mediator.Send(new DeliverGoodsCommand(notification.Order.Id), cancellationToken);
        }
    }
}