using PaymentGateway.Domain.AggregatesModel.DeliverAggregate;

namespace PaymentGateway.Infrastructure.Repositories;

public interface IDeliverRecordRepository : IRepository<DeliverRecord, DeliverRecordId>
{
}

public class DeliverRecordRepository(ApplicationDbContext context) : RepositoryBase<DeliverRecord, DeliverRecordId, ApplicationDbContext>(context), IDeliverRecordRepository
{
}

