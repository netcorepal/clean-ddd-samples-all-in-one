using FastEndpoints;
using RiskControl.Web.Application.Commands;
using RiskControl.Domain.AggregatesModel.OrderAggregate;
using NetCorePal.Extensions.Dto;

namespace RiskControl.Web.Endpoints.RiskEndpoints;

public class RaiseComplianceAlertRequest
{
    public long OrderId { get; set; }
    public string RuleCode { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string? AutoResolution { get; set; }
}

[Tags("Risk")]
[HttpPost("/api/risk/compliance-alert")] 
public class RaiseComplianceAlertEndpoint(IMediator mediator) : Endpoint<RaiseComplianceAlertRequest, ResponseData<string>>
{
    public override async Task HandleAsync(RaiseComplianceAlertRequest req, CancellationToken ct)
    {
        var id = await mediator.Send(new RaiseComplianceAlertCommand(new OrderId(req.OrderId), req.RuleCode, req.Detail, req.AutoResolution), ct);
        await Send.OkAsync(id.ToString().AsResponseData(), cancellation: ct);
    }
}
