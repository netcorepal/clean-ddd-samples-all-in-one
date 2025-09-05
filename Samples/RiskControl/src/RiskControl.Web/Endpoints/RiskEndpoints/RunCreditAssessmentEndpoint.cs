using FastEndpoints;
using RiskControl.Web.Application.Commands;
using RiskControl.Domain.AggregatesModel.OrderAggregate;
using NetCorePal.Extensions.Dto;

namespace RiskControl.Web.Endpoints.RiskEndpoints;

public class RunCreditAssessmentRequest
{
    public long OrderId { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public decimal Exposure { get; set; }
}

[Tags("Risk")]
[HttpPost("/api/risk/credit-assessment")] 
public class RunCreditAssessmentEndpoint(IMediator mediator) : Endpoint<RunCreditAssessmentRequest, ResponseData<string>>
{
    public override async Task HandleAsync(RunCreditAssessmentRequest req, CancellationToken ct)
    {
        var id = await mediator.Send(new RunCreditAssessmentCommand(new OrderId(req.OrderId), req.CustomerId, req.Exposure), ct);
        await Send.OkAsync(id.ToString().AsResponseData(), cancellation: ct);
    }
}
