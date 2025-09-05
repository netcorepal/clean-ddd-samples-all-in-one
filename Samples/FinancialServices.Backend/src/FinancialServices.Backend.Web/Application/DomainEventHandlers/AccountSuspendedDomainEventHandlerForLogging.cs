using FinancialServices.Backend.Domain.DomainEvents;

namespace FinancialServices.Backend.Web.Application.DomainEventHandlers;

public class AccountSuspendedDomainEventHandlerForLogging(
    ILogger<AccountSuspendedDomainEventHandlerForLogging> logger)
    : IDomainEventHandler<AccountSuspendedDomainEvent>
{
    public Task Handle(AccountSuspendedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var account = domainEvent.Account;
        logger.LogWarning("账户已被暂停：AccountId={AccountId}, Email={Email}, Reason={Reason}", 
            account.Id, account.Email, domainEvent.Reason);
        
        // 这里可以添加通知用户和相关系统的逻辑
        // 例如：发送账户暂停通知、记录安全日志等
        
        return Task.CompletedTask;
    }
}
