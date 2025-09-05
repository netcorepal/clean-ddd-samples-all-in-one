using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using FinancialServices.Backend.Web.Application.Commands.Accounts;
using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;

namespace FinancialServices.Backend.Web.Endpoints.Accounts;

public record SuspendAccountRequestDto(AccountId AccountId, string Reason);

public record ActivateAccountRequestDto(AccountId AccountId);

public record CloseAccountRequestDto(AccountId AccountId);

[Tags("Account")]
[HttpPost("/api/accounts/{AccountId}/suspend")]
[AllowAnonymous]
public class SuspendAccountEndpoint(IMediator mediator) : Endpoint<SuspendAccountRequestDto, EmptyResponse>
{
    public override async Task HandleAsync(SuspendAccountRequestDto req, CancellationToken ct)
    {
        var command = new SuspendAccountCommand(req.AccountId, req.Reason);
        await mediator.Send(command, ct);
        
        await Send.NoContentAsync(ct);
    }
}

[Tags("Account")]
[HttpPost("/api/accounts/{AccountId}/activate")]
[AllowAnonymous]
public class ActivateAccountEndpoint(IMediator mediator) : Endpoint<ActivateAccountRequestDto, EmptyResponse>
{
    public override async Task HandleAsync(ActivateAccountRequestDto req, CancellationToken ct)
    {
        var command = new ActivateAccountCommand(req.AccountId);
        await mediator.Send(command, ct);
        
        await Send.NoContentAsync(ct);
    }
}

[Tags("Account")]
[HttpPost("/api/accounts/{AccountId}/close")]
[AllowAnonymous]
public class CloseAccountEndpoint(IMediator mediator) : Endpoint<CloseAccountRequestDto, EmptyResponse>
{
    public override async Task HandleAsync(CloseAccountRequestDto req, CancellationToken ct)
    {
        var command = new CloseAccountCommand(req.AccountId);
        await mediator.Send(command, ct);
        
        await Send.NoContentAsync(ct);
    }
}
