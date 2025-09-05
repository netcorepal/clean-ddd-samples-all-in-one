using TradingEngine.Domain.AggregatesModel.DeliverAggregate;
using TradingEngine.Domain.AggregatesModel.OrderAggregate;
using TradingEngine.Infrastructure.Repositories;
using NetCorePal.Extensions.Primitives;

namespace TradingEngine.Web.Application.Commands;

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