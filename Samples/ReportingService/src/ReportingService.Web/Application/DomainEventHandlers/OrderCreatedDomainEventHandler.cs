using ReportingService.Domain.DomainEvents;
using ReportingService.Web.Application.Commands;
using MediatR;
using NetCorePal.Extensions.Domain;

namespace ReportingService.Web.Application.DomainEventHandlers
{
    internal class OrderCreatedDomainEventHandler(IMediator mediator) : IDomainEventHandler<OrderCreatedDomainEvent>
    {
        public Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            return mediator.Send(new DeliverGoodsCommand(notification.Order.Id), cancellationToken);
        }
    }
}