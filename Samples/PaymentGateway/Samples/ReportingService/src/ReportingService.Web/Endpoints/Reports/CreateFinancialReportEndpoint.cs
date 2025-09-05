using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using ReportingService.Domain.AggregatesModel.FinancialReportAggregate;
using ReportingService.Web.Application.Commands.Reports;
using ReportingService.Web.Application.Queries.Reports;

namespace ReportingService.Web.Endpoints.Reports;

public record CreateFinancialReportRequest(string Title, string Period, string? Content);
public record CreateFinancialReportResponse(FinancialReportId ReportId);

[Tags("Reports")]
[HttpPost("/api/reports/financial")] 
[Authorize(AuthenticationSchemes = "Bearer")]
public class CreateFinancialReportEndpoint(IMediator mediator) : Endpoint<CreateFinancialReportRequest, ResponseData<CreateFinancialReportResponse>>
{
    public override async Task HandleAsync(CreateFinancialReportRequest req, CancellationToken ct)
    {
        var id = await mediator.Send(new CreateFinancialReportCommand(req.Title, req.Period, req.Content), ct);
        await Send.OkAsync(new CreateFinancialReportResponse(id).AsResponseData(), ct);
    }
}

[Tags("Reports")]
[HttpGet("/api/reports/financial/{reportId:long}")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class GetFinancialReportEndpoint(IMediator mediator) : EndpointWithoutRequest<ResponseData<FinancialReportDto>>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
    var raw = Route<long>("reportId");
    var id = new FinancialReportId(raw);
        var dto = await mediator.Send(new GetFinancialReportQuery(id), ct);
        await Send.OkAsync(dto.AsResponseData(), ct);
    }
}
