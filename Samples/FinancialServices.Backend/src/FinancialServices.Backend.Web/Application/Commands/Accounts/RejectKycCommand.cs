using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;
using FinancialServices.Backend.Infrastructure.Repositories;

namespace FinancialServices.Backend.Web.Application.Commands.Accounts;

public record RejectKycCommand(AccountId AccountId, string RejectionReason) : ICommand;

public class RejectKycCommandValidator : AbstractValidator<RejectKycCommand>
{
    public RejectKycCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("账户ID不能为空");

        RuleFor(x => x.RejectionReason)
            .NotEmpty()
            .WithMessage("拒绝原因不能为空")
            .MaximumLength(500)
            .WithMessage("拒绝原因不能超过500个字符");
    }
}

public class RejectKycCommandHandler(IAccountRepository accountRepository)
    : ICommandHandler<RejectKycCommand>
{
    public async Task Handle(RejectKycCommand command, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetAsync(command.AccountId, cancellationToken) ??
                     throw new KnownException($"未找到账户，AccountId = {command.AccountId}");

        account.RejectKyc(command.RejectionReason);
    }
}
