using FinancialServices.Backend.Domain.AggregatesModel.DeliverAggregate;

namespace FinancialServices.Backend.Infrastructure.Repositories;

public interface IDeliverRecordRepository : IRepository<DeliverRecord, DeliverRecordId>
{
}

public class DeliverRecordRepository(ApplicationDbContext context) : RepositoryBase<DeliverRecord, DeliverRecordId, ApplicationDbContext>(context), IDeliverRecordRepository
{
}

