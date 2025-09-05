using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;
using FinancialServices.Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FinancialServices.Backend.Web.Application.Queries.Accounts;

public record GetAccountsQuery(
    int PageIndex = 1,
    int PageSize = 10,
    string? Email = null,
    AccountStatus? Status = null,
    KycStatus? KycStatus = null,
    bool CountTotal = true) : IQuery<PagedData<AccountSummaryDto>>;

public class GetAccountsQueryValidator : AbstractValidator<GetAccountsQuery>
{
    public GetAccountsQueryValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0)
            .WithMessage("页码必须大于0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("每页条数必须在1-100之间");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("邮箱格式不正确");
    }
}

public class GetAccountsQueryHandler(ApplicationDbContext context)
    : IQueryHandler<GetAccountsQuery, PagedData<AccountSummaryDto>>
{
    public async Task<PagedData<AccountSummaryDto>> Handle(GetAccountsQuery query, CancellationToken cancellationToken)
    {
        var queryable = context.Accounts.AsNoTracking();

        // 条件过滤
        queryable = queryable
            .WhereIf(!string.IsNullOrEmpty(query.Email), x => x.Email.Contains(query.Email!))
            .WhereIf(query.Status.HasValue, x => x.Status == query.Status!.Value)
            .WhereIf(query.KycStatus.HasValue, x => x.KycStatus == query.KycStatus!.Value);

        // 投影和排序
        var result = queryable
            .Select(x => new AccountSummaryDto(
                x.Id,
                x.Email,
                x.FullName,
                x.Status,
                x.KycStatus,
                x.CreatedAt))
            .OrderByDescending(x => x.CreatedAt);

        return await result.ToPagedDataAsync(query.PageIndex, query.PageSize, query.CountTotal, cancellationToken);
    }
}
