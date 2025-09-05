using ReportingService.Domain.AggregatesModel.AnalysisAggregate;
using ReportingService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ReportingService.Web.Application.Queries.Analysis;

public record GetAnalysisRecordQuery(AnalysisRecordId AnalysisId) : IQuery<AnalysisRecordDto>;

public record AnalysisRecordDto(AnalysisRecordId Id, string Name, string Parameters, string Status, DateTimeOffset StartedAt, DateTimeOffset? CompletedAt, string? Result);

public class GetAnalysisRecordQueryValidator : AbstractValidator<GetAnalysisRecordQuery>
{
    public GetAnalysisRecordQueryValidator()
    {
        RuleFor(x => x.AnalysisId).NotEmpty();
    }
}

public class GetAnalysisRecordQueryHandler(ApplicationDbContext context)
    : IQueryHandler<GetAnalysisRecordQuery, AnalysisRecordDto>
{
    public async Task<AnalysisRecordDto> Handle(GetAnalysisRecordQuery request, CancellationToken cancellationToken)
    {
        var dto = await context.AnalysisRecords
            .Where(x => x.Id == request.AnalysisId)
            .Select(x => new AnalysisRecordDto(x.Id, x.Name, x.Parameters, x.Status, x.StartedAt, x.CompletedAt, x.Result))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new KnownException($"未找到分析记录：{request.AnalysisId}");
        return dto;
    }
}
