using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using FinancialServices.Backend.Web.Application.Queries.Accounts;
using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;

namespace FinancialServices.Backend.Web.Endpoints.Accounts;

public record GetAccountRequestDto(AccountId AccountId);

[Tags("Account")]
[HttpGet("/api/accounts/{AccountId}")]
[AllowAnonymous]
public class GetAccountEndpoint(IMediator mediator) : Endpoint<GetAccountRequestDto, ResponseData<AccountDto>>
{
    public override async Task HandleAsync(GetAccountRequestDto req, CancellationToken ct)
    {
        var query = new GetAccountQuery(req.AccountId);
        var account = await mediator.Send(query, ct);
        
        await Send.OkAsync(account.AsResponseData(), ct);
    }
}
