using FinancialServices.Backend.Domain.DomainEvents;

namespace FinancialServices.Backend.Web.Application.DomainEventHandlers;

public class AccountCreatedDomainEventHandlerForLogging(
    ILogger<AccountCreatedDomainEventHandlerForLogging> logger)
    : IDomainEventHandler<AccountCreatedDomainEvent>
{
    public Task Handle(AccountCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var account = domainEvent.Account;
        logger.LogInformation("新账户已创建：AccountId={AccountId}, Email={Email}, FullName={FullName}", 
            account.Id, account.Email, account.FullName);
        
        return Task.CompletedTask;
    }
}
