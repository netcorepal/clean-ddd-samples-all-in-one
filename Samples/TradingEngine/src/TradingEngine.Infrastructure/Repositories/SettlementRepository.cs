using TradingEngine.Domain.AggregatesModel.SettlementAggregate;

namespace TradingEngine.Infrastructure.Repositories;

public interface ISettlementRepository : IRepository<Settlement, SettlementId>
{
    Task<IEnumerable<Settlement>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Settlement>> GetByStatusAsync(SettlementStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Settlement>> GetPendingSettlementsAsync(CancellationToken cancellationToken = default);
}

public class SettlementRepository : RepositoryBase<Settlement, SettlementId, ApplicationDbContext>, ISettlementRepository
{
    public SettlementRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Settlement>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Settlement>()
            .Include(s => s.Items)
            .Where(s => s.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Settlement>> GetByStatusAsync(SettlementStatus status, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Settlement>()
            .Include(s => s.Items)
            .Where(s => s.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Settlement>> GetPendingSettlementsAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Settlement>()
            .Include(s => s.Items)
            .Where(s => s.Status == SettlementStatus.Pending)
            .ToListAsync(cancellationToken);
    }
}
