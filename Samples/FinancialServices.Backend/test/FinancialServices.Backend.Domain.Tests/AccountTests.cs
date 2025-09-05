using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;
using FinancialServices.Backend.Domain.DomainEvents;

namespace FinancialServices.Backend.Domain.Tests;

public class AccountTests
{
    [Fact]
    public void CreateAccount_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var email = "test@example.com";
        var fullName = "Test User";
        var phoneNumber = "13800138000";

        // Act
        var account = new Account(email, fullName, phoneNumber);

        // Assert
        Assert.Equal(email, account.Email);
        Assert.Equal(fullName, account.FullName);
        Assert.Equal(phoneNumber, account.PhoneNumber);
        Assert.Equal(AccountStatus.Pending, account.Status);
        Assert.Equal(KycStatus.NotStarted, account.KycStatus);
        Assert.True((DateTime.UtcNow - account.CreatedAt).TotalSeconds < 1);
    }

    [Fact]
    public void SubmitKycVerification_WhenNotStarted_ShouldUpdateStatus()
    {
        // Arrange
        var account = new Account("test@example.com", "Test User", "13800138000");
        var documentType = "身份证";
        var documentNumber = "123456789012345678";

        // Act
        account.SubmitKycVerification(documentType, documentNumber);

        // Assert
        Assert.Equal(documentType, account.IdentityDocumentType);
        Assert.Equal(documentNumber, account.IdentityDocumentNumber);
        Assert.Equal(KycStatus.InProgress, account.KycStatus);
        Assert.NotNull(account.KycSubmittedAt);
        Assert.Null(account.KycRejectionReason);
    }

    [Fact]
    public void ApproveKyc_WhenInProgress_ShouldActivateAccount()
    {
        // Arrange
        var account = new Account("test@example.com", "Test User", "13800138000");
        account.SubmitKycVerification("身份证", "123456789012345678");

        // Act
        account.ApproveKyc();

        // Assert
        Assert.Equal(KycStatus.Approved, account.KycStatus);
        Assert.Equal(AccountStatus.Active, account.Status);
        Assert.NotNull(account.KycApprovedAt);
    }

    [Fact]
    public void RejectKyc_WhenInProgress_ShouldSetRejectionReason()
    {
        // Arrange
        var account = new Account("test@example.com", "Test User", "13800138000");
        account.SubmitKycVerification("身份证", "123456789012345678");
        var rejectionReason = "身份证信息不清晰";

        // Act
        account.RejectKyc(rejectionReason);

        // Assert
        Assert.Equal(KycStatus.Rejected, account.KycStatus);
        Assert.Equal(rejectionReason, account.KycRejectionReason);
    }

    [Fact]
    public void SuspendAccount_WhenActive_ShouldUpdateStatus()
    {
        // Arrange
        var account = new Account("test@example.com", "Test User", "13800138000");
        account.SubmitKycVerification("身份证", "123456789012345678");
        account.ApproveKyc();
        var reason = "异常交易";

        // Act
        account.SuspendAccount(reason);

        // Assert
        Assert.Equal(AccountStatus.Suspended, account.Status);
    }

    [Fact]
    public void ApproveKyc_WhenNotInProgress_ShouldThrowException()
    {
        // Arrange
        var account = new Account("test@example.com", "Test User", "13800138000");

        // Act & Assert
        var exception = Assert.Throws<KnownException>(() => account.ApproveKyc());
        Assert.Contains("KYC verification can only be approved when status is InProgress", exception.Message);
    }

    [Fact]
    public void ActivateAccount_WithoutApprovedKyc_ShouldThrowException()
    {
        // Arrange
        var account = new Account("test@example.com", "Test User", "13800138000");

        // Act & Assert
        var exception = Assert.Throws<KnownException>(() => account.ActivateAccount());
        Assert.Contains("Cannot activate account without approved KYC", exception.Message);
    }
}
