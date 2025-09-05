using PaymentGateway.Domain.AggregatesModel.RefundAggregate;

namespace PaymentGateway.Infrastructure.Repositories;

public interface IRefundRepository : IRepository<Refund, RefundId>
{
}

public class RefundRepository(ApplicationDbContext context) : RepositoryBase<Refund, RefundId, ApplicationDbContext>(context), IRefundRepository
{
}
