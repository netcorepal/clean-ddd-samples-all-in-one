using TradingEngine.Domain.AggregatesModel.SettlementAggregate;
using TradingEngine.Web.Application.Queries.Settlement;
using FastEndpoints;

namespace TradingEngine.Web.Endpoints.SettlementEndpoints;

public record GetSettlementDetailRequest(SettlementId SettlementId);

[Tags("Settlement")]
[HttpGet("/api/settlements/{settlementId}")]
public class GetSettlementDetailEndpoint(IMediator mediator) : Endpoint<GetSettlementDetailRequest, ResponseData<SettlementDetailDto>>
{
    public override async Task HandleAsync(GetSettlementDetailRequest req, CancellationToken ct)
    {
        var query = new GetSettlementDetailQuery(req.SettlementId);
        var settlement = await mediator.Send(query, ct);
        
        await Send.OkAsync(settlement.AsResponseData(), cancellation: ct);
    }
}
