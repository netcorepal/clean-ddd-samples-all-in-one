using TradingEngine.Domain.AggregatesModel.TradeAggregate;
using TradingEngine.Web.Application.Queries.Trade;
using FastEndpoints;

namespace TradingEngine.Web.Endpoints.TradeEndpoints;

public record GetUserTradesRequest(
    int PageIndex = 1, 
    int PageSize = 20, 
    TradeStatus? Status = null, 
    string? Symbol = null);

[Tags("Trades")]
[HttpGet("/api/trades")]
public class GetUserTradesEndpoint(IMediator mediator) : Endpoint<GetUserTradesRequest, ResponseData<PagedData<TradeListDto>>>
{
    public override async Task HandleAsync(GetUserTradesRequest req, CancellationToken ct)
    {
        // 从JWT中获取用户ID
        var userId = HttpContext.User.FindFirst("name")?.Value ?? "default-user";
        
        var query = new GetTradesByUserQuery(userId, req.PageIndex, req.PageSize, req.Status, req.Symbol);
        var trades = await mediator.Send(query, ct);
        
        await Send.OkAsync(trades.AsResponseData(), cancellation: ct);
    }
}
