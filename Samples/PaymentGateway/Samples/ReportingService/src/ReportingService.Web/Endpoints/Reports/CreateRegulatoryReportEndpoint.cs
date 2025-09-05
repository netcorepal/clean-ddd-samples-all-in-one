using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using ReportingService.Domain.AggregatesModel.RegulatoryReportAggregate;
using ReportingService.Web.Application.Commands.Reports;
using ReportingService.Web.Application.Queries.Reports;

namespace ReportingService.Web.Endpoints.Reports;

public record CreateRegulatoryReportRequest(string Category, string Period, string? Payload);
public record CreateRegulatoryReportResponse(RegulatoryReportId ReportId);

[Tags("Reports")]
[HttpPost("/api/reports/regulatory")] 
[Authorize(AuthenticationSchemes = "Bearer")]
public class CreateRegulatoryReportEndpoint(IMediator mediator) : Endpoint<CreateRegulatoryReportRequest, ResponseData<CreateRegulatoryReportResponse>>
{
    public override async Task HandleAsync(CreateRegulatoryReportRequest req, CancellationToken ct)
    {
        var id = await mediator.Send(new CreateRegulatoryReportCommand(req.Category, req.Period, req.Payload), ct);
        await Send.OkAsync(new CreateRegulatoryReportResponse(id).AsResponseData(), ct);
    }
}

[Tags("Reports")]
[HttpGet("/api/reports/regulatory/{reportId:long}")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class GetRegulatoryReportEndpoint(IMediator mediator) : EndpointWithoutRequest<ResponseData<RegulatoryReportDto>>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
    var raw = Route<long>("reportId");
    var id = new RegulatoryReportId(raw);
        var dto = await mediator.Send(new GetRegulatoryReportQuery(id), ct);
        await Send.OkAsync(dto.AsResponseData(), ct);
    }
}
