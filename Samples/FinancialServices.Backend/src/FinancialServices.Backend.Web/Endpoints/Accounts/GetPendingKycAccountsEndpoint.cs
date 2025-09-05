using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using FinancialServices.Backend.Web.Application.Queries.Accounts;

namespace FinancialServices.Backend.Web.Endpoints.Accounts;

public record GetPendingKycAccountsRequestDto(
    int PageIndex = 1,
    int PageSize = 10,
    bool CountTotal = true);

[Tags("Account")]
[HttpGet("/api/accounts/kyc/pending")]
[AllowAnonymous]
public class GetPendingKycAccountsEndpoint(IMediator mediator) : Endpoint<GetPendingKycAccountsRequestDto, ResponseData<PagedData<PendingKycAccountDto>>>
{
    public override async Task HandleAsync(GetPendingKycAccountsRequestDto req, CancellationToken ct)
    {
        var query = new GetPendingKycAccountsQuery(req.PageIndex, req.PageSize, req.CountTotal);
        var accounts = await mediator.Send(query, ct);
        
        await Send.OkAsync(accounts.AsResponseData(), ct);
    }
}
