using PaymentGateway.Domain.AggregatesModel.ReconciliationAggregate;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Infrastructure.Repositories;

namespace PaymentGateway.Web.Application.Commands.Reconciliation;

public record ImportReconciliationRecordCommand(string Provider, string ProviderTransactionId, decimal Amount, string Currency, DateTimeOffset OccurTime) : ICommand<ReconciliationRecordId>;

public class ImportReconciliationRecordCommandValidator : AbstractValidator<ImportReconciliationRecordCommand>
{
    public ImportReconciliationRecordCommandValidator()
    {
        RuleFor(x => x.Provider).NotEmpty();
        RuleFor(x => x.ProviderTransactionId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Currency).NotEmpty();
    }
}

public class ImportReconciliationRecordCommandHandler(IReconciliationRecordRepository repository)
    : ICommandHandler<ImportReconciliationRecordCommand, ReconciliationRecordId>
{
    public async Task<ReconciliationRecordId> Handle(ImportReconciliationRecordCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByProviderAsync(request.Provider, request.ProviderTransactionId, cancellationToken);
        if (existing is not null)
        {
            return existing.Id; // idempotent import
        }

        var record = new ReconciliationRecord(request.Provider, request.ProviderTransactionId, request.Amount, request.Currency, request.OccurTime);
        await repository.AddAsync(record, cancellationToken);
        return record.Id;
    }
}

public record MatchReconciliationRecordCommand(ReconciliationRecordId RecordId, PaymentId PaymentId, bool Mismatch = false, string? Note = null) : ICommand;

public class MatchReconciliationRecordCommandValidator : AbstractValidator<MatchReconciliationRecordCommand>
{
    public MatchReconciliationRecordCommandValidator()
    {
        RuleFor(x => x.RecordId).NotEmpty();
        RuleFor(x => x.PaymentId).NotEmpty().When(x => !x.Mismatch);
    }
}

public class MatchReconciliationRecordCommandHandler(IReconciliationRecordRepository repository)
    : ICommandHandler<MatchReconciliationRecordCommand>
{
    public async Task Handle(MatchReconciliationRecordCommand request, CancellationToken cancellationToken)
    {
        var record = await repository.GetAsync(request.RecordId, cancellationToken)
                     ?? throw new KnownException($"Reconciliation record not found, Id = {request.RecordId}");

        if (request.Mismatch)
        {
            record.MarkMismatch(request.Note ?? "mismatch");
        }
        else
        {
            record.MarkMatched(request.PaymentId);
        }

        await repository.UpdateAsync(record, cancellationToken);
    }
}
