using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;

namespace PaymentGateway.Infrastructure.Repositories;

public interface IPaymentRepository : IRepository<Payment, PaymentId>
{
    Task<Payment?> GetByProviderTxnAsync(string providerTransactionId, CancellationToken cancellationToken = default);
}

public class PaymentRepository : RepositoryBase<Payment, PaymentId, ApplicationDbContext>, IPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<Payment?> GetByProviderTxnAsync(string providerTransactionId, CancellationToken cancellationToken = default)
    {
        return _context.Set<Payment>().FirstOrDefaultAsync(x => x.ProviderTransactionId == providerTransactionId, cancellationToken);
    }
}
