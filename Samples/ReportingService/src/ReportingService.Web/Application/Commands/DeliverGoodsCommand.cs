using ReportingService.Domain.AggregatesModel.DeliverAggregate;
using ReportingService.Domain.AggregatesModel.OrderAggregate;
using ReportingService.Infrastructure.Repositories;
using NetCorePal.Extensions.Primitives;

namespace ReportingService.Web.Application.Commands;

public record DeliverGoodsCommand(OrderId OrderId) : ICommand<DeliverRecordId>;

public class DeliverGoodsCommandHandler(IDeliverRecordRepository deliverRecordRepository)
    : ICommandHandler<DeliverGoodsCommand, DeliverRecordId>
{
    public Task<DeliverRecordId> Handle(DeliverGoodsCommand request, CancellationToken cancellationToken)
    {
        var record = new DeliverRecord(request.OrderId);
        deliverRecordRepository.Add(record);
        return Task.FromResult(record.Id);
    }
}