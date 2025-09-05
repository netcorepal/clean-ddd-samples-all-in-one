using FinancialServices.Backend.Domain.AggregatesModel.DeliverAggregate;
using FinancialServices.Backend.Domain.AggregatesModel.OrderAggregate;
using FinancialServices.Backend.Infrastructure.Repositories;
using NetCorePal.Extensions.Primitives;

namespace FinancialServices.Backend.Web.Application.Commands;

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