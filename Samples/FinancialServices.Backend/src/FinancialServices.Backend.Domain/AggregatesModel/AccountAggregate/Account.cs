using FinancialServices.Backend.Domain.DomainEvents;

namespace FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;

// 强类型ID定义
public partial record AccountId : IGuidStronglyTypedId;

// 账户状态枚举
public enum AccountStatus
{
    Pending = 0,      // 待审核
    Active = 1,       // 活跃
    Suspended = 2,    // 暂停
    Closed = 3        // 关闭
}

// KYC状态枚举
public enum KycStatus
{
    NotStarted = 0,   // 未开始
    InProgress = 1,   // 进行中
    Approved = 2,     // 已通过
    Rejected = 3      // 已拒绝
}

public class Account : Entity<AccountId>, IAggregateRoot
{
    protected Account() { }
    
    public Account(string email, string fullName, string phoneNumber)
    {
        Email = email;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Status = AccountStatus.Pending;
        KycStatus = KycStatus.NotStarted;
        CreatedAt = DateTime.UtcNow;
        
        this.AddDomainEvent(new AccountCreatedDomainEvent(this));
    }

    #region Properties

    public string Email { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public AccountStatus Status { get; private set; } = AccountStatus.Pending;
    public KycStatus KycStatus { get; private set; } = KycStatus.NotStarted;
    public DateTime CreatedAt { get; private set; }
    public DateTime? KycSubmittedAt { get; private set; }
    public DateTime? KycApprovedAt { get; private set; }
    public string? KycRejectionReason { get; private set; }
    public string? IdentityDocumentType { get; private set; }
    public string? IdentityDocumentNumber { get; private set; }
    public RowVersion RowVersion { get; private set; } = new RowVersion(0);

    #endregion

    #region Methods

    public void SubmitKycVerification(string identityDocumentType, string identityDocumentNumber)
    {
        if (KycStatus != KycStatus.NotStarted && KycStatus != KycStatus.Rejected)
        {
            throw new KnownException("KYC verification can only be submitted when status is NotStarted or Rejected");
        }

        IdentityDocumentType = identityDocumentType;
        IdentityDocumentNumber = identityDocumentNumber;
        KycStatus = KycStatus.InProgress;
        KycSubmittedAt = DateTime.UtcNow;
        KycRejectionReason = null;

        this.AddDomainEvent(new AccountKycSubmittedDomainEvent(this));
    }

    public void ApproveKyc()
    {
        if (KycStatus != KycStatus.InProgress)
        {
            throw new KnownException("KYC verification can only be approved when status is InProgress");
        }

        KycStatus = KycStatus.Approved;
        KycApprovedAt = DateTime.UtcNow;
        Status = AccountStatus.Active;

        this.AddDomainEvent(new AccountKycApprovedDomainEvent(this));
    }

    public void RejectKyc(string rejectionReason)
    {
        if (KycStatus != KycStatus.InProgress)
        {
            throw new KnownException("KYC verification can only be rejected when status is InProgress");
        }

        KycStatus = KycStatus.Rejected;
        KycRejectionReason = rejectionReason;

        this.AddDomainEvent(new AccountKycRejectedDomainEvent(this));
    }

    public void SuspendAccount(string reason)
    {
        if (Status == AccountStatus.Closed)
        {
            throw new KnownException("Cannot suspend a closed account");
        }

        Status = AccountStatus.Suspended;
        this.AddDomainEvent(new AccountSuspendedDomainEvent(this, reason));
    }

    public void ActivateAccount()
    {
        if (Status == AccountStatus.Closed)
        {
            throw new KnownException("Cannot activate a closed account");
        }

        if (KycStatus != KycStatus.Approved)
        {
            throw new KnownException("Cannot activate account without approved KYC");
        }

        Status = AccountStatus.Active;
        this.AddDomainEvent(new AccountActivatedDomainEvent(this));
    }

    public void CloseAccount()
    {
        Status = AccountStatus.Closed;
        this.AddDomainEvent(new AccountClosedDomainEvent(this));
    }

    public void UpdateContactInfo(string email, string phoneNumber)
    {
        Email = email;
        PhoneNumber = phoneNumber;
        this.AddDomainEvent(new AccountContactInfoUpdatedDomainEvent(this));
    }

    #endregion
}
