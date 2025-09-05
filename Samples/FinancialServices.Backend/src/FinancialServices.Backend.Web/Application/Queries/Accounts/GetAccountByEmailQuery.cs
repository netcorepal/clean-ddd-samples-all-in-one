using FinancialServices.Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FinancialServices.Backend.Web.Application.Queries.Accounts;

public record GetAccountByEmailQuery(string Email) : IQuery<AccountDto?>;

public class GetAccountByEmailQueryValidator : AbstractValidator<GetAccountByEmailQuery>
{
    public GetAccountByEmailQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("邮箱不能为空")
            .EmailAddress()
            .WithMessage("邮箱格式不正确");
    }
}

public class GetAccountByEmailQueryHandler(ApplicationDbContext context)
    : IQueryHandler<GetAccountByEmailQuery, AccountDto?>
{
    public async Task<AccountDto?> Handle(GetAccountByEmailQuery query, CancellationToken cancellationToken)
    {
        return await context.Accounts
            .AsNoTracking()
            .Where(x => x.Email == query.Email)
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
    }
}
