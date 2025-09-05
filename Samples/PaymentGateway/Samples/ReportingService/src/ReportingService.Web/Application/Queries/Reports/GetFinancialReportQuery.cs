using ReportingService.Domain.AggregatesModel.FinancialReportAggregate;
using ReportingService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ReportingService.Web.Application.Queries.Reports;

public record GetFinancialReportQuery(FinancialReportId ReportId) : IQuery<FinancialReportDto>;

public record FinancialReportDto(FinancialReportId Id, string Title, string Period, string Content, DateTimeOffset GeneratedAt);

public class GetFinancialReportQueryValidator : AbstractValidator<GetFinancialReportQuery>
{
    public GetFinancialReportQueryValidator()
    {
        RuleFor(x => x.ReportId).NotEmpty();
    }
}

public class GetFinancialReportQueryHandler(ApplicationDbContext context)
    : IQueryHandler<GetFinancialReportQuery, FinancialReportDto>
{
    public async Task<FinancialReportDto> Handle(GetFinancialReportQuery request, CancellationToken cancellationToken)
    {
        var dto = await context.FinancialReports
            .Where(x => x.Id == request.ReportId)
            .Select(x => new FinancialReportDto(x.Id, x.Title, x.Period, x.Content, x.GeneratedAt))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new KnownException($"未找到财务报表：{request.ReportId}");
        return dto;
    }
}
