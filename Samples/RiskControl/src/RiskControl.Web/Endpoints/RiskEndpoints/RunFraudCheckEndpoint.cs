using FastEndpoints;
using RiskControl.Web.Application.Commands;
using RiskControl.Domain.AggregatesModel.FraudAggregate;
using NetCorePal.Extensions.Dto;
using RiskControl.Domain.AggregatesModel.OrderAggregate;

namespace RiskControl.Web.Endpoints.RiskEndpoints;

public class RunFraudCheckRequest
{
    public long OrderId { get; set; }
    public string Channel { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string IpAddress { get; set; } = string.Empty;
}

[Tags("Risk")]
[HttpPost("/api/risk/fraud-check")] 
public class RunFraudCheckEndpoint(IMediator mediator) : Endpoint<RunFraudCheckRequest, ResponseData<string>>
{
    public override async Task HandleAsync(RunFraudCheckRequest req, CancellationToken ct)
    {
        var id = await mediator.Send(new RunFraudCheckCommand(new OrderId(req.OrderId), req.Channel, req.Amount, req.IpAddress), ct);
        await Send.OkAsync(id.ToString().AsResponseData(), cancellation: ct);
    }
}
