using FinancialServices.Backend.Domain.DomainEvents;

namespace FinancialServices.Backend.Web.Application.DomainEventHandlers;

public class AccountKycApprovedDomainEventHandlerForNotification(
    ILogger<AccountKycApprovedDomainEventHandlerForNotification> logger)
    : IDomainEventHandler<AccountKycApprovedDomainEvent>
{
    public Task Handle(AccountKycApprovedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var account = domainEvent.Account;
        logger.LogInformation("账户KYC验证已通过，账户已激活：AccountId={AccountId}, Email={Email}", 
            account.Id, account.Email);
        
        // 这里可以添加通知用户的逻辑
        // 例如：发送祝贺邮件、短信通知等
        
        return Task.CompletedTask;
    }
}
