using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;

namespace FinancialServices.Backend.Domain.DomainEvents;

public record AccountCreatedDomainEvent(Account Account) : IDomainEvent;

public record AccountKycSubmittedDomainEvent(Account Account) : IDomainEvent;

public record AccountKycApprovedDomainEvent(Account Account) : IDomainEvent;

public record AccountKycRejectedDomainEvent(Account Account) : IDomainEvent;

public record AccountSuspendedDomainEvent(Account Account, string Reason) : IDomainEvent;

public record AccountActivatedDomainEvent(Account Account) : IDomainEvent;

public record AccountClosedDomainEvent(Account Account) : IDomainEvent;

public record AccountContactInfoUpdatedDomainEvent(Account Account) : IDomainEvent;
