using TradingEngine.Domain.AggregatesModel.SettlementAggregate;
using TradingEngine.Web.Application.Commands.Settlement;
using FastEndpoints;

namespace TradingEngine.Web.Endpoints.SettlementEndpoints;

public record ProcessSettlementRequest(SettlementId SettlementId);

[Tags("Settlement")]
[HttpPost("/api/settlements/{settlementId}/process")]
public class ProcessSettlementEndpoint(IMediator mediator) : Endpoint<ProcessSettlementRequest, ResponseData>
{
    public override async Task HandleAsync(ProcessSettlementRequest req, CancellationToken ct)
    {
        var command = new ProcessSettlementCommand(req.SettlementId);
        await mediator.Send(command, ct);
        
        await Send.OkAsync(new ResponseData(), cancellation: ct);
    }
}
