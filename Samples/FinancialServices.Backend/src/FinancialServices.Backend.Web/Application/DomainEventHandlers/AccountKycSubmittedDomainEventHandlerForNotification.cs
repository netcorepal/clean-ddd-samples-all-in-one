using FinancialServices.Backend.Domain.DomainEvents;

namespace FinancialServices.Backend.Web.Application.DomainEventHandlers;

public class AccountKycSubmittedDomainEventHandlerForNotification(
    ILogger<AccountKycSubmittedDomainEventHandlerForNotification> logger)
    : IDomainEventHandler<AccountKycSubmittedDomainEvent>
{
    public Task Handle(AccountKycSubmittedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var account = domainEvent.Account;
        logger.LogInformation("账户KYC验证已提交，等待审核：AccountId={AccountId}, Email={Email}, DocumentType={DocumentType}", 
            account.Id, account.Email, account.IdentityDocumentType);
        
        // 这里可以添加发送通知给审核人员的逻辑
        // 例如：发送邮件、短信或推送通知
        
        return Task.CompletedTask;
    }
}
