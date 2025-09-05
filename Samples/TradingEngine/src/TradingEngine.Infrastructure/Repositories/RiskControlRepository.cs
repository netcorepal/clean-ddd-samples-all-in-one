using TradingEngine.Domain.AggregatesModel.RiskControlAggregate;

namespace TradingEngine.Infrastructure.Repositories;

public interface IRiskControlRepository : IRepository<RiskControl, RiskControlId>
{
    Task<RiskControl?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RiskControl>> GetActiveRiskControlsAsync(CancellationToken cancellationToken = default);
}

public class RiskControlRepository : RepositoryBase<RiskControl, RiskControlId, ApplicationDbContext>, IRiskControlRepository
{
    public RiskControlRepository(ApplicationDbContext context) : base(context) { }

    public async Task<RiskControl?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<RiskControl>()
            .Include(rc => rc.RiskAssessments)
            .FirstOrDefaultAsync(rc => rc.UserId == userId, cancellationToken);
    }

    public async Task<IEnumerable<RiskControl>> GetActiveRiskControlsAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<RiskControl>()
            .Include(rc => rc.RiskAssessments)
            .Where(rc => rc.IsActive)
            .ToListAsync(cancellationToken);
    }
}
