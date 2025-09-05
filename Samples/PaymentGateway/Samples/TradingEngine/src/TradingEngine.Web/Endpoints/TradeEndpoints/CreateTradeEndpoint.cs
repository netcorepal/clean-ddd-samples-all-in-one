using TradingEngine.Domain.AggregatesModel.TradeAggregate;
using TradingEngine.Web.Application.Commands.Trade;
using FastEndpoints;

namespace TradingEngine.Web.Endpoints.TradeEndpoints;

public record CreateTradeRequest(string Symbol, TradeType TradeType, decimal Quantity, decimal Price);

[Tags("Trades")]
[HttpPost("/api/trades")]
public class CreateTradeEndpoint(IMediator mediator) : Endpoint<CreateTradeRequest, ResponseData<TradeId>>
{
    public override async Task HandleAsync(CreateTradeRequest req, CancellationToken ct)
    {
        // 从JWT中获取用户ID
        var userId = HttpContext.User.FindFirst("name")?.Value ?? "default-user";
        
        var command = new CreateTradeCommand(req.Symbol, req.TradeType, req.Quantity, req.Price, userId);
        var tradeId = await mediator.Send(command, ct);
        
        await Send.OkAsync(tradeId.AsResponseData(), cancellation: ct);
    }
}
