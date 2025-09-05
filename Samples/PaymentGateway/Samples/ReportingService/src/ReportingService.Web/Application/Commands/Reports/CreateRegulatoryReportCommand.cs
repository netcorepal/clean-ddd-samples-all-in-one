using ReportingService.Domain.AggregatesModel.RegulatoryReportAggregate;
using ReportingService.Infrastructure.Repositories;

namespace ReportingService.Web.Application.Commands.Reports;

public record CreateRegulatoryReportCommand(string Category, string Period, string? Payload) : ICommand<RegulatoryReportId>;

public class CreateRegulatoryReportCommandValidator : AbstractValidator<CreateRegulatoryReportCommand>
{
    public CreateRegulatoryReportCommandValidator()
    {
        RuleFor(x => x.Category).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Period).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Payload).MaximumLength(4000);
    }
}

public class CreateRegulatoryReportCommandHandler(IRegulatoryReportRepository repo)
    : ICommandHandler<CreateRegulatoryReportCommand, RegulatoryReportId>
{
    public async Task<RegulatoryReportId> Handle(CreateRegulatoryReportCommand request, CancellationToken cancellationToken)
    {
        var report = new RegulatoryReport(request.Category, request.Period, request.Payload);
        await repo.AddAsync(report, cancellationToken);
        return report.Id;
    }
}
