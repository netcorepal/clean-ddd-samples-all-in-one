using TradingEngine.Web.Application.Queries.RiskControl;
using FastEndpoints;

namespace TradingEngine.Web.Endpoints.RiskControlEndpoints;

[Tags("RiskControl")]
[HttpGet("/api/risk-control")]
public class GetRiskControlEndpoint(IMediator mediator) : EndpointWithoutRequest<ResponseData<RiskControlDto>>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        // 从JWT中获取用户ID
        var userId = HttpContext.User.FindFirst("name")?.Value ?? "default-user";
        
        var query = new GetRiskControlByUserQuery(userId);
        var riskControl = await mediator.Send(query, ct);
        
        await Send.OkAsync(riskControl.AsResponseData(), cancellation: ct);
    }
}
