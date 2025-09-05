using PaymentGateway.Domain.AggregatesModel.ReconciliationAggregate;

namespace PaymentGateway.Infrastructure.Repositories;

public interface IReconciliationRecordRepository : IRepository<ReconciliationRecord, ReconciliationRecordId>
{
    Task<ReconciliationRecord?> GetByProviderAsync(string provider, string providerTransactionId, CancellationToken cancellationToken = default);
}

public class ReconciliationRecordRepository : RepositoryBase<ReconciliationRecord, ReconciliationRecordId, ApplicationDbContext>, IReconciliationRecordRepository
{
    private readonly ApplicationDbContext _context;

    public ReconciliationRecordRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<ReconciliationRecord?> GetByProviderAsync(string provider, string providerTransactionId, CancellationToken cancellationToken = default)
    {
        return _context.Set<ReconciliationRecord>()
            .FirstOrDefaultAsync(x => x.Provider == provider && x.ProviderTransactionId == providerTransactionId, cancellationToken);
    }
}
