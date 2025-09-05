using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;
using FinancialServices.Backend.Infrastructure.Repositories;

namespace FinancialServices.Backend.Web.Application.Commands.Accounts;

public record UpdateAccountContactInfoCommand(
    AccountId AccountId,
    string Email,
    string PhoneNumber) : ICommand;

public class UpdateAccountContactInfoCommandValidator : AbstractValidator<UpdateAccountContactInfoCommand>
{
    public UpdateAccountContactInfoCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("账户ID不能为空");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("邮箱不能为空")
            .EmailAddress()
            .WithMessage("邮箱格式不正确")
            .MaximumLength(100)
            .WithMessage("邮箱不能超过100个字符");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("手机号不能为空")
            .Matches(@"^1[3-9]\d{9}$")
            .WithMessage("手机号格式不正确")
            .MaximumLength(20)
            .WithMessage("手机号不能超过20个字符");
    }
}

public class UpdateAccountContactInfoCommandHandler(IAccountRepository accountRepository)
    : ICommandHandler<UpdateAccountContactInfoCommand>
{
    public async Task Handle(UpdateAccountContactInfoCommand command, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetAsync(command.AccountId, cancellationToken) ??
                     throw new KnownException($"未找到账户，AccountId = {command.AccountId}");

        // 检查新邮箱是否被其他账户使用
        var existingAccountByEmail = await accountRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existingAccountByEmail != null && existingAccountByEmail.Id != command.AccountId)
        {
            throw new KnownException("邮箱已被其他账户使用");
        }

        // 检查新手机号是否被其他账户使用
        var existingAccountByPhone = await accountRepository.GetByPhoneNumberAsync(command.PhoneNumber, cancellationToken);
        if (existingAccountByPhone != null && existingAccountByPhone.Id != command.AccountId)
        {
            throw new KnownException("手机号已被其他账户使用");
        }

        account.UpdateContactInfo(command.Email, command.PhoneNumber);
    }
}
