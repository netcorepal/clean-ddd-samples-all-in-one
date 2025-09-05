using TradingEngine.Domain.AggregatesModel.TradeAggregate;
using TradingEngine.Web.Application.Commands.Trade;
using FastEndpoints;

namespace TradingEngine.Web.Endpoints.TradeEndpoints;

[Tags("Trades")]
[HttpPost("/api/trades/{tradeId}/cancel")]
public class CancelTradeEndpoint(IMediator mediator) : EndpointWithoutRequest<ResponseData>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        var tradeId = Route<TradeId>("tradeId");
        if (tradeId == null)
        {
            throw new KnownException("无效的交易ID");
        }
        
        var command = new CancelTradeCommand(tradeId);
        await mediator.Send(command, ct);
        
        await Send.OkAsync(new ResponseData(), cancellation: ct);
    }
}
