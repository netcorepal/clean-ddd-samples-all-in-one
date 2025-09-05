using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;
using FinancialServices.Backend.Infrastructure.Repositories;

namespace FinancialServices.Backend.Web.Application.Commands.Accounts;

public record CreateAccountCommand(string Email, string FullName, string PhoneNumber) : ICommand<AccountId>;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("邮箱不能为空")
            .EmailAddress()
            .WithMessage("邮箱格式不正确")
            .MaximumLength(100)
            .WithMessage("邮箱不能超过100个字符");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("姓名不能为空")
            .MaximumLength(100)
            .WithMessage("姓名不能超过100个字符");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("手机号不能为空")
            .Matches(@"^1[3-9]\d{9}$")
            .WithMessage("手机号格式不正确")
            .MaximumLength(20)
            .WithMessage("手机号不能超过20个字符");
    }
}

public class CreateAccountCommandHandler(IAccountRepository accountRepository)
    : ICommandHandler<CreateAccountCommand, AccountId>
{
    public async Task<AccountId> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        // 检查邮箱是否已存在
        if (await accountRepository.GetByEmailAsync(command.Email, cancellationToken) != null)
        {
            throw new KnownException("邮箱已被注册");
        }

        // 检查手机号是否已存在
        if (await accountRepository.GetByPhoneNumberAsync(command.PhoneNumber, cancellationToken) != null)
        {
            throw new KnownException("手机号已被注册");
        }

        var account = new Account(command.Email, command.FullName, command.PhoneNumber);
        await accountRepository.AddAsync(account, cancellationToken);
        
        return account.Id;
    }
}
