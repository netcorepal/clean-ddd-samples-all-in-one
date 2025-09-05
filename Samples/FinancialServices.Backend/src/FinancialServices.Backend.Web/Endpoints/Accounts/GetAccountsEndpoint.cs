using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using FinancialServices.Backend.Web.Application.Queries.Accounts;
using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;

namespace FinancialServices.Backend.Web.Endpoints.Accounts;

public record GetAccountsRequestDto(
    int PageIndex = 1,
    int PageSize = 10,
    string? Email = null,
    AccountStatus? Status = null,
    KycStatus? KycStatus = null,
    bool CountTotal = true);

[Tags("Account")]
[HttpGet("/api/accounts")]
[AllowAnonymous]
public class GetAccountsEndpoint(IMediator mediator) : Endpoint<GetAccountsRequestDto, ResponseData<PagedData<AccountSummaryDto>>>
{
    public override async Task HandleAsync(GetAccountsRequestDto req, CancellationToken ct)
    {
        var query = new GetAccountsQuery(req.PageIndex, req.PageSize, req.Email, req.Status, req.KycStatus, req.CountTotal);
        var accounts = await mediator.Send(query, ct);
        
        await Send.OkAsync(accounts.AsResponseData(), ct);
    }
}
