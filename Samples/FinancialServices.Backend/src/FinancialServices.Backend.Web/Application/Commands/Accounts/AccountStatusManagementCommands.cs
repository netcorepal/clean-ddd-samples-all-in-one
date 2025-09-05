using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;
using FinancialServices.Backend.Infrastructure.Repositories;

namespace FinancialServices.Backend.Web.Application.Commands.Accounts;

public record SuspendAccountCommand(AccountId AccountId, string Reason) : ICommand;

public class SuspendAccountCommandValidator : AbstractValidator<SuspendAccountCommand>
{
    public SuspendAccountCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("账户ID不能为空");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("暂停原因不能为空")
            .MaximumLength(500)
            .WithMessage("暂停原因不能超过500个字符");
    }
}

public class SuspendAccountCommandHandler(IAccountRepository accountRepository)
    : ICommandHandler<SuspendAccountCommand>
{
    public async Task Handle(SuspendAccountCommand command, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetAsync(command.AccountId, cancellationToken) ??
                     throw new KnownException($"未找到账户，AccountId = {command.AccountId}");

        account.SuspendAccount(command.Reason);
    }
}

public record ActivateAccountCommand(AccountId AccountId) : ICommand;

public class ActivateAccountCommandValidator : AbstractValidator<ActivateAccountCommand>
{
    public ActivateAccountCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("账户ID不能为空");
    }
}

public class ActivateAccountCommandHandler(IAccountRepository accountRepository)
    : ICommandHandler<ActivateAccountCommand>
{
    public async Task Handle(ActivateAccountCommand command, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetAsync(command.AccountId, cancellationToken) ??
                     throw new KnownException($"未找到账户，AccountId = {command.AccountId}");

        account.ActivateAccount();
    }
}

public record CloseAccountCommand(AccountId AccountId) : ICommand;

public class CloseAccountCommandValidator : AbstractValidator<CloseAccountCommand>
{
    public CloseAccountCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("账户ID不能为空");
    }
}

public class CloseAccountCommandHandler(IAccountRepository accountRepository)
    : ICommandHandler<CloseAccountCommand>
{
    public async Task Handle(CloseAccountCommand command, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetAsync(command.AccountId, cancellationToken) ??
                     throw new KnownException($"未找到账户，AccountId = {command.AccountId}");

        account.CloseAccount();
    }
}
