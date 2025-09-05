using TradingEngine.Domain.AggregatesModel.RiskControlAggregate;
using TradingEngine.Web.Application.Commands.RiskControl;
using FastEndpoints;

namespace TradingEngine.Web.Endpoints.RiskControlEndpoints;

public record CreateRiskControlRequest(decimal TotalPositionLimit, decimal DailyLossLimit);

[Tags("RiskControl")]
[HttpPost("/api/risk-control")]
public class CreateRiskControlEndpoint(IMediator mediator) : Endpoint<CreateRiskControlRequest, ResponseData<RiskControlId>>
{
    public override async Task HandleAsync(CreateRiskControlRequest req, CancellationToken ct)
    {
        // 从JWT中获取用户ID
        var userId = HttpContext.User.FindFirst("name")?.Value ?? "default-user";
        
        var command = new CreateRiskControlCommand(userId, req.TotalPositionLimit, req.DailyLossLimit);
        var riskControlId = await mediator.Send(command, ct);
        
        await Send.OkAsync(riskControlId.AsResponseData(), cancellation: ct);
    }
}
