using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using FinancialServices.Backend.Web.Application.Commands.Accounts;
using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;

namespace FinancialServices.Backend.Web.Endpoints.Accounts;

public record ApproveKycRequestDto(AccountId AccountId);

public record RejectKycRequestDto(AccountId AccountId, string RejectionReason);

[Tags("Account")]
[HttpPost("/api/accounts/{AccountId}/kyc/approve")]
[AllowAnonymous]
public class ApproveKycEndpoint(IMediator mediator) : Endpoint<ApproveKycRequestDto, EmptyResponse>
{
    public override async Task HandleAsync(ApproveKycRequestDto req, CancellationToken ct)
    {
        var command = new ApproveKycCommand(req.AccountId);
        await mediator.Send(command, ct);
        
        await Send.NoContentAsync(ct);
    }
}

[Tags("Account")]
[HttpPost("/api/accounts/{AccountId}/kyc/reject")]
[AllowAnonymous]
public class RejectKycEndpoint(IMediator mediator) : Endpoint<RejectKycRequestDto, EmptyResponse>
{
    public override async Task HandleAsync(RejectKycRequestDto req, CancellationToken ct)
    {
        var command = new RejectKycCommand(req.AccountId, req.RejectionReason);
        await mediator.Send(command, ct);
        
        await Send.NoContentAsync(ct);
    }
}
