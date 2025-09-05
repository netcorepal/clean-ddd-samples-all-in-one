using TradingEngine.Domain.AggregatesModel.DeliverAggregate;

namespace TradingEngine.Infrastructure.Repositories;

public interface IDeliverRecordRepository : IRepository<DeliverRecord, DeliverRecordId>
{
}

public class DeliverRecordRepository(ApplicationDbContext context) : RepositoryBase<DeliverRecord, DeliverRecordId, ApplicationDbContext>(context), IDeliverRecordRepository
{
}

