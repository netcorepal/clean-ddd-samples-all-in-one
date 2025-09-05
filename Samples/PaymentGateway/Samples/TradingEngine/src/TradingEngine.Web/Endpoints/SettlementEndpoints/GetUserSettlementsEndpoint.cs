using TradingEngine.Domain.AggregatesModel.SettlementAggregate;
using TradingEngine.Web.Application.Queries.Settlement;
using FastEndpoints;

namespace TradingEngine.Web.Endpoints.SettlementEndpoints;

public record GetUserSettlementsRequest(
    int PageIndex = 1, 
    int PageSize = 20, 
    SettlementStatus? Status = null);

[Tags("Settlement")]
[HttpGet("/api/settlements")]
public class GetUserSettlementsEndpoint(IMediator mediator) : Endpoint<GetUserSettlementsRequest, ResponseData<PagedData<SettlementListDto>>>
{
    public override async Task HandleAsync(GetUserSettlementsRequest req, CancellationToken ct)
    {
        // 从JWT中获取用户ID
        var userId = HttpContext.User.FindFirst("name")?.Value ?? "default-user";
        
        var query = new GetSettlementsByUserQuery(userId, req.PageIndex, req.PageSize, req.Status);
        var settlements = await mediator.Send(query, ct);
        
        await Send.OkAsync(settlements.AsResponseData(), cancellation: ct);
    }
}
