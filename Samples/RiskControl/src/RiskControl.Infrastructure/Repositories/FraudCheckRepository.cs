using RiskControl.Domain.AggregatesModel.FraudAggregate;

namespace RiskControl.Infrastructure.Repositories;

public interface IFraudCheckRepository : IRepository<FraudCheck, FraudCheckId>
{
}

public class FraudCheckRepository(ApplicationDbContext context) : RepositoryBase<FraudCheck, FraudCheckId, ApplicationDbContext>(context), IFraudCheckRepository
{
}
