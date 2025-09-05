using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using FinancialServices.Backend.Web.Application.Commands.Accounts;
using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;

namespace FinancialServices.Backend.Web.Endpoints.Accounts;

public record CreateAccountRequestDto(string Email, string FullName, string PhoneNumber);

public record CreateAccountResponseDto(AccountId AccountId);

[Tags("Account")]
[HttpPost("/api/accounts")]
[AllowAnonymous]
public class CreateAccountEndpoint(IMediator mediator) : Endpoint<CreateAccountRequestDto, ResponseData<CreateAccountResponseDto>>
{
    public override async Task HandleAsync(CreateAccountRequestDto req, CancellationToken ct)
    {
        var command = new CreateAccountCommand(req.Email, req.FullName, req.PhoneNumber);
        var accountId = await mediator.Send(command, ct);
        
        var response = new CreateAccountResponseDto(accountId);
        await Send.OkAsync(response.AsResponseData(), ct);
    }
}
