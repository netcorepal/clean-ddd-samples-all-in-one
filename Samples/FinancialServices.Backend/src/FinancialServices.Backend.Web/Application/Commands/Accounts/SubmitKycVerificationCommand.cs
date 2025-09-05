using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;
using FinancialServices.Backend.Infrastructure.Repositories;

namespace FinancialServices.Backend.Web.Application.Commands.Accounts;

public record SubmitKycVerificationCommand(
    AccountId AccountId,
    string IdentityDocumentType,
    string IdentityDocumentNumber) : ICommand;

public class SubmitKycVerificationCommandValidator : AbstractValidator<SubmitKycVerificationCommand>
{
    public SubmitKycVerificationCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("账户ID不能为空");

        RuleFor(x => x.IdentityDocumentType)
            .NotEmpty()
            .WithMessage("身份证件类型不能为空")
            .MaximumLength(50)
            .WithMessage("身份证件类型不能超过50个字符");

        RuleFor(x => x.IdentityDocumentNumber)
            .NotEmpty()
            .WithMessage("身份证件号码不能为空")
            .MaximumLength(100)
            .WithMessage("身份证件号码不能超过100个字符");
    }
}

public class SubmitKycVerificationCommandHandler(IAccountRepository accountRepository)
    : ICommandHandler<SubmitKycVerificationCommand>
{
    public async Task Handle(SubmitKycVerificationCommand command, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetAsync(command.AccountId, cancellationToken) ??
                     throw new KnownException($"未找到账户，AccountId = {command.AccountId}");

        account.SubmitKycVerification(command.IdentityDocumentType, command.IdentityDocumentNumber);
    }
}
