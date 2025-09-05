using ReportingService.Domain.AggregatesModel.FinancialReportAggregate;
using ReportingService.Infrastructure.Repositories;

namespace ReportingService.Web.Application.Commands.Reports;

public record CreateFinancialReportCommand(string Title, string Period, string? Content) : ICommand<FinancialReportId>;

public class CreateFinancialReportCommandValidator : AbstractValidator<CreateFinancialReportCommand>
{
    public CreateFinancialReportCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Period).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Content).MaximumLength(4000);
    }
}

public class CreateFinancialReportCommandHandler(IFinancialReportRepository repo)
    : ICommandHandler<CreateFinancialReportCommand, FinancialReportId>
{
    public async Task<FinancialReportId> Handle(CreateFinancialReportCommand request, CancellationToken cancellationToken)
    {
        var report = new FinancialReport(request.Title, request.Period, request.Content);
        await repo.AddAsync(report, cancellationToken);
        return report.Id;
    }
}
