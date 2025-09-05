using FinancialServices.Backend.Domain.DomainEvents;

namespace FinancialServices.Backend.Web.Application.DomainEventHandlers;

public class AccountKycRejectedDomainEventHandlerForNotification(
    ILogger<AccountKycRejectedDomainEventHandlerForNotification> logger)
    : IDomainEventHandler<AccountKycRejectedDomainEvent>
{
    public Task Handle(AccountKycRejectedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var account = domainEvent.Account;
        logger.LogInformation("账户KYC验证被拒绝：AccountId={AccountId}, Email={Email}, Reason={Reason}", 
            account.Id, account.Email, account.KycRejectionReason);
        
        // 这里可以添加通知用户的逻辑
        // 例如：发送拒绝原因邮件、短信通知等
        
        return Task.CompletedTask;
    }
}
