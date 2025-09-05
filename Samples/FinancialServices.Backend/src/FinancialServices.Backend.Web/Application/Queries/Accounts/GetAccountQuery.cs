using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;
using FinancialServices.Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FinancialServices.Backend.Web.Application.Queries.Accounts;

public record GetAccountQuery(AccountId AccountId) : IQuery<AccountDto>;

public class GetAccountQueryValidator : AbstractValidator<GetAccountQuery>
{
    public GetAccountQueryValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("账户ID不能为空");
    }
}

public class GetAccountQueryHandler(ApplicationDbContext context)
    : IQueryHandler<GetAccountQuery, AccountDto>
{
    public async Task<AccountDto> Handle(GetAccountQuery query, CancellationToken cancellationToken)
    {
        var accountDto = await context.Accounts
            .Where(x => x.Id == query.AccountId)
            .Select(x => new AccountDto(
                x.Id,
                x.Email,
                x.FullName,
                x.PhoneNumber,
                x.Status,
                x.KycStatus,
                x.CreatedAt,
                x.KycSubmittedAt,
                x.KycApprovedAt,
                x.KycRejectionReason,
                x.IdentityDocumentType,
                x.IdentityDocumentNumber))
            .FirstOrDefaultAsync(cancellationToken);

        return accountDto ?? throw new KnownException($"未找到账户，AccountId = {query.AccountId}");
    }
}
