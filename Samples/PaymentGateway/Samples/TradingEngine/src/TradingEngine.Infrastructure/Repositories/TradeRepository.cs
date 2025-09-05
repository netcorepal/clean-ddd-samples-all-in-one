using TradingEngine.Domain.AggregatesModel.TradeAggregate;

namespace TradingEngine.Infrastructure.Repositories;

public interface ITradeRepository : IRepository<Trade, TradeId>
{
    Task<IEnumerable<Trade>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Trade>> GetBySymbolAsync(string symbol, CancellationToken cancellationToken = default);
    Task<IEnumerable<Trade>> GetPendingTradesAsync(CancellationToken cancellationToken = default);
}

public class TradeRepository : RepositoryBase<Trade, TradeId, ApplicationDbContext>, ITradeRepository
{
    public TradeRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Trade>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Trade>().Where(t => t.UserId == userId).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Trade>> GetBySymbolAsync(string symbol, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Trade>().Where(t => t.Symbol == symbol).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Trade>> GetPendingTradesAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Trade>().Where(t => t.Status == TradeStatus.Pending || t.Status == TradeStatus.PartiallyFilled)
            .ToListAsync(cancellationToken);
    }
}
