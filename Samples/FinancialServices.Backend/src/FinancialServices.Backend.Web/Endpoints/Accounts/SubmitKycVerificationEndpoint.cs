using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using FinancialServices.Backend.Web.Application.Commands.Accounts;
using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;

namespace FinancialServices.Backend.Web.Endpoints.Accounts;

public record SubmitKycVerificationRequestDto(
    AccountId AccountId,
    string IdentityDocumentType,
    string IdentityDocumentNumber);

[Tags("Account")]
[HttpPost("/api/accounts/{AccountId}/kyc/submit")]
[AllowAnonymous]
public class SubmitKycVerificationEndpoint(IMediator mediator) : Endpoint<SubmitKycVerificationRequestDto, EmptyResponse>
{
    public override async Task HandleAsync(SubmitKycVerificationRequestDto req, CancellationToken ct)
    {
        var command = new SubmitKycVerificationCommand(req.AccountId, req.IdentityDocumentType, req.IdentityDocumentNumber);
        await mediator.Send(command, ct);
        
        await Send.NoContentAsync(ct);
    }
}
