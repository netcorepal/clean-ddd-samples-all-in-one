using TradingEngine.Domain.AggregatesModel.TradeAggregate;
using TradingEngine.Web.Application.Queries.Trade;
using FastEndpoints;

namespace TradingEngine.Web.Endpoints.TradeEndpoints;

public record GetTradeRequest(TradeId TradeId);

[Tags("Trades")]
[HttpGet("/api/trades/{tradeId}")]
public class GetTradeEndpoint(IMediator mediator) : Endpoint<GetTradeRequest, ResponseData<TradeDto>>
{
    public override async Task HandleAsync(GetTradeRequest req, CancellationToken ct)
    {
        var query = new GetTradeQuery(req.TradeId);
        var trade = await mediator.Send(query, ct);
        
        await Send.OkAsync(trade.AsResponseData(), cancellation: ct);
    }
}
