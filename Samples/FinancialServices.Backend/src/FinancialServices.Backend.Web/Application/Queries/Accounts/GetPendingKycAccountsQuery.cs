using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;
using FinancialServices.Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FinancialServices.Backend.Web.Application.Queries.Accounts;

public record GetPendingKycAccountsQuery(
    int PageIndex = 1,
    int PageSize = 10,
    bool CountTotal = true) : IQuery<PagedData<PendingKycAccountDto>>;

public class GetPendingKycAccountsQueryValidator : AbstractValidator<GetPendingKycAccountsQuery>
{
    public GetPendingKycAccountsQueryValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0)
            .WithMessage("页码必须大于0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("每页条数必须在1-100之间");
    }
}

public class GetPendingKycAccountsQueryHandler(ApplicationDbContext context)
    : IQueryHandler<GetPendingKycAccountsQuery, PagedData<PendingKycAccountDto>>
{
    public async Task<PagedData<PendingKycAccountDto>> Handle(GetPendingKycAccountsQuery query, CancellationToken cancellationToken)
    {
        var result = context.Accounts
            .AsNoTracking()
            .Where(x => x.KycStatus == KycStatus.InProgress)
            .Select(x => new PendingKycAccountDto(
                x.Id,
                x.Email,
                x.FullName,
                x.PhoneNumber,
                x.KycSubmittedAt,
                x.IdentityDocumentType,
                x.IdentityDocumentNumber))
            .OrderBy(x => x.KycSubmittedAt);

        return await result.ToPagedDataAsync(query.PageIndex, query.PageSize, query.CountTotal, cancellationToken);
    }
}
