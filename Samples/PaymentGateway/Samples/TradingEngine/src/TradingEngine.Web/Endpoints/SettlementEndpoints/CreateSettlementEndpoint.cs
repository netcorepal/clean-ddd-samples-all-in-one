using TradingEngine.Domain.AggregatesModel.SettlementAggregate;
using TradingEngine.Web.Application.Commands.Settlement;
using FastEndpoints;

namespace TradingEngine.Web.Endpoints.SettlementEndpoints;

public record CreateSettlementRequest(SettlementType SettlementType, DateTimeOffset SettlementDate);

[Tags("Settlement")]
[HttpPost("/api/settlements")]
public class CreateSettlementEndpoint(IMediator mediator) : Endpoint<CreateSettlementRequest, ResponseData<SettlementId>>
{
    public override async Task HandleAsync(CreateSettlementRequest req, CancellationToken ct)
    {
        // 从JWT中获取用户ID
        var userId = HttpContext.User.FindFirst("name")?.Value ?? "default-user";
        
        var command = new CreateSettlementCommand(userId, req.SettlementType, req.SettlementDate);
        var settlementId = await mediator.Send(command, ct);
        
        await Send.OkAsync(settlementId.AsResponseData(), cancellation: ct);
    }
}
