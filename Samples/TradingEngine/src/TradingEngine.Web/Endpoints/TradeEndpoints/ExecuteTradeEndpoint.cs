using TradingEngine.Domain.AggregatesModel.TradeAggregate;
using TradingEngine.Web.Application.Commands.Trade;
using FastEndpoints;

namespace TradingEngine.Web.Endpoints.TradeEndpoints;

public record ExecuteTradeRequest(decimal ExecutedQuantity, decimal ExecutedPrice);

[Tags("Trades")]
[HttpPost("/api/trades/{tradeId}/execute")]
public class ExecuteTradeEndpoint(IMediator mediator) : Endpoint<ExecuteTradeRequest, ResponseData>
{
    public override async Task HandleAsync(ExecuteTradeRequest req, CancellationToken ct)
    {
        var tradeId = Route<TradeId>("tradeId");
        if (tradeId == null)
        {
            throw new KnownException("无效的交易ID");
        }
        
        var command = new ExecuteTradeCommand(tradeId, req.ExecutedQuantity, req.ExecutedPrice);
        await mediator.Send(command, ct);
        
        await Send.OkAsync(new ResponseData(), cancellation: ct);
    }
}
