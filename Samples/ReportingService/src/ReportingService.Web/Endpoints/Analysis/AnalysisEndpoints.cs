using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using ReportingService.Domain.AggregatesModel.AnalysisAggregate;
using ReportingService.Web.Application.Commands.Analysis;
using ReportingService.Web.Application.Queries.Analysis;

namespace ReportingService.Web.Endpoints.Analysis;

public record StartAnalysisRequest(string Name, string? Parameters);
public record StartAnalysisResponse(AnalysisRecordId AnalysisId);

[Tags("Analysis")]
[HttpPost("/api/analysis")] 
[Authorize(AuthenticationSchemes = "Bearer")]
public class StartAnalysisEndpoint(IMediator mediator) : Endpoint<StartAnalysisRequest, ResponseData<StartAnalysisResponse>>
{
    public override async Task HandleAsync(StartAnalysisRequest req, CancellationToken ct)
    {
        var id = await mediator.Send(new StartAnalysisCommand(req.Name, req.Parameters), ct);
        await Send.OkAsync(new StartAnalysisResponse(id).AsResponseData(), ct);
    }
}

[Tags("Analysis")]
[HttpGet("/api/analysis/{analysisId:long}")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class GetAnalysisEndpoint(IMediator mediator) : EndpointWithoutRequest<ResponseData<AnalysisRecordDto>>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
    var raw = Route<long>("analysisId");
    var id = new AnalysisRecordId(raw);
        var dto = await mediator.Send(new GetAnalysisRecordQuery(id), ct);
        await Send.OkAsync(dto.AsResponseData(), ct);
    }
}

[Tags("Analysis")]
[HttpPost("/api/analysis/{analysisId:long}/complete")] 
[Authorize(AuthenticationSchemes = "Bearer")]
public class CompleteAnalysisEndpoint(IMediator mediator) : Endpoint<CompleteAnalysisRequest, EmptyResponse>
{
    public override async Task HandleAsync(CompleteAnalysisRequest req, CancellationToken ct)
    {
    var raw = Route<long>("analysisId");
    var id = new AnalysisRecordId(raw);
        await mediator.Send(new CompleteAnalysisCommand(id, req.Result), ct);
        await Send.NoContentAsync(ct);
    }
}

public record CompleteAnalysisRequest(string Result);
