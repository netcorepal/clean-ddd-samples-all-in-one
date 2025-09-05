using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using PaymentGateway.Web.Application.Commands.Reconciliation;
using PaymentGateway.Domain.AggregatesModel.ReconciliationAggregate;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;

namespace PaymentGateway.Web.Endpoints.Reconciliation;

public record ImportReconciliationRecordRequest(string Provider, string ProviderTransactionId, decimal Amount, string Currency, DateTimeOffset OccurTime);
public record ImportReconciliationRecordResponse(ReconciliationRecordId Id);

[AllowAnonymous]
[HttpPost("/api/reconciliation/import")]
public class ImportReconciliationRecordEndpoint(IMediator mediator) : Endpoint<ImportReconciliationRecordRequest, ResponseData<ImportReconciliationRecordResponse>>
{
    public override async Task HandleAsync(ImportReconciliationRecordRequest req, CancellationToken ct)
    {
        var id = await mediator.Send(new ImportReconciliationRecordCommand(req.Provider, req.ProviderTransactionId, req.Amount, req.Currency, req.OccurTime), ct);
        await Send.OkAsync(new ImportReconciliationRecordResponse(id).AsResponseData(), ct);
    }
}

public record MatchReconciliationRecordRequest(ReconciliationRecordId RecordId, PaymentId? PaymentId, bool Mismatch, string? Note);

[AllowAnonymous]
[HttpPost("/api/reconciliation/match")]
public class MatchReconciliationRecordEndpoint(IMediator mediator) : Endpoint<MatchReconciliationRecordRequest, EmptyResponse>
{
    public override async Task HandleAsync(MatchReconciliationRecordRequest req, CancellationToken ct)
    {
    var paymentId = req.PaymentId ?? default!;
    await mediator.Send(new MatchReconciliationRecordCommand(req.RecordId, paymentId, req.Mismatch, req.Note), ct);
        await Send.NoContentAsync(ct);
    }
}
