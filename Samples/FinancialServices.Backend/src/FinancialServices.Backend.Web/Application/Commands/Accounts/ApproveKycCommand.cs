using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;
using FinancialServices.Backend.Infrastructure.Repositories;

namespace FinancialServices.Backend.Web.Application.Commands.Accounts;

public record ApproveKycCommand(AccountId AccountId) : ICommand;

public class ApproveKycCommandValidator : AbstractValidator<ApproveKycCommand>
{
    public ApproveKycCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("账户ID不能为空");
    }
}

public class ApproveKycCommandHandler(IAccountRepository accountRepository)
    : ICommandHandler<ApproveKycCommand>
{
    public async Task Handle(ApproveKycCommand command, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetAsync(command.AccountId, cancellationToken) ??
                     throw new KnownException($"未找到账户，AccountId = {command.AccountId}");

        account.ApproveKyc();
    }
}
