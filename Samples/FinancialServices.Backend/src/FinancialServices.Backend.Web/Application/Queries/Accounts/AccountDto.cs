using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;

namespace FinancialServices.Backend.Web.Application.Queries.Accounts;

public record AccountDto(
    AccountId Id,
    string Email,
    string FullName,
    string PhoneNumber,
    AccountStatus Status,
    KycStatus KycStatus,
    DateTime CreatedAt,
    DateTime? KycSubmittedAt,
    DateTime? KycApprovedAt,
    string? KycRejectionReason,
    string? IdentityDocumentType,
    string? IdentityDocumentNumber);

public record AccountSummaryDto(
    AccountId Id,
    string Email,
    string FullName,
    AccountStatus Status,
    KycStatus KycStatus,
    DateTime CreatedAt);

public record PendingKycAccountDto(
    AccountId Id,
    string Email,
    string FullName,
    string PhoneNumber,
    DateTime? KycSubmittedAt,
    string? IdentityDocumentType,
    string? IdentityDocumentNumber);
