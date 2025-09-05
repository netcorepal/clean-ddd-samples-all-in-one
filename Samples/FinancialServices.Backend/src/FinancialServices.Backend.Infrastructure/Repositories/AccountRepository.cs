using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;

namespace FinancialServices.Backend.Infrastructure.Repositories;

// 接口和实现定义在同一文件中
public interface IAccountRepository : IRepository<Account, AccountId>
{
    /// <summary>
    /// 根据邮箱获取账户
    /// </summary>
    /// <param name="email">邮箱地址</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>账户实体，如果不存在则返回null</returns>
    Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据手机号获取账户
    /// </summary>
    /// <param name="phoneNumber">手机号</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>账户实体，如果不存在则返回null</returns>
    Task<Account?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取待审核的KYC账户列表
    /// </summary>
    /// <param name="pageIndex">页索引（从0开始）</param>
    /// <param name="pageSize">页大小</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>待审核的KYC账户列表</returns>
    Task<List<Account>> GetPendingKycAccountsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default);
}

public class AccountRepository(ApplicationDbContext context) : RepositoryBase<Account, AccountId, ApplicationDbContext>(context), IAccountRepository
{
    public async Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Accounts.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<Account?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await context.Accounts.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<List<Account>> GetPendingKycAccountsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        return await context.Accounts
            .Where(x => x.KycStatus == KycStatus.InProgress)
            .OrderBy(x => x.KycSubmittedAt)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}
