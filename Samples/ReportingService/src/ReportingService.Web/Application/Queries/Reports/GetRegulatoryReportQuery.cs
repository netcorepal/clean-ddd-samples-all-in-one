using ReportingService.Domain.AggregatesModel.RegulatoryReportAggregate;
using ReportingService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ReportingService.Web.Application.Queries.Reports;

public record GetRegulatoryReportQuery(RegulatoryReportId ReportId) : IQuery<RegulatoryReportDto>;

public record RegulatoryReportDto(RegulatoryReportId Id, string Category, string Period, string Payload, bool Submitted);

public class GetRegulatoryReportQueryValidator : AbstractValidator<GetRegulatoryReportQuery>
{
    public GetRegulatoryReportQueryValidator()
    {
        RuleFor(x => x.ReportId).NotEmpty();
    }
}

public class GetRegulatoryReportQueryHandler(ApplicationDbContext context)
    : IQueryHandler<GetRegulatoryReportQuery, RegulatoryReportDto>
{
    public async Task<RegulatoryReportDto> Handle(GetRegulatoryReportQuery request, CancellationToken cancellationToken)
    {
        var dto = await context.RegulatoryReports
            .Where(x => x.Id == request.ReportId)
            .Select(x => new RegulatoryReportDto(x.Id, x.Category, x.Period, x.Payload, x.Submitted))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new KnownException($"未找到监管报告：{request.ReportId}");
        return dto;
    }
}
