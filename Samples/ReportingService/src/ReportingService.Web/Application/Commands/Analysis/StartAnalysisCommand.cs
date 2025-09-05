using ReportingService.Domain.AggregatesModel.AnalysisAggregate;
using ReportingService.Infrastructure.Repositories;

namespace ReportingService.Web.Application.Commands.Analysis;

public record StartAnalysisCommand(string Name, string? Parameters) : ICommand<AnalysisRecordId>;

public class StartAnalysisCommandValidator : AbstractValidator<StartAnalysisCommand>
{
    public StartAnalysisCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Parameters).MaximumLength(2000);
    }
}

public class StartAnalysisCommandHandler(IAnalysisRecordRepository repo)
    : ICommandHandler<StartAnalysisCommand, AnalysisRecordId>
{
    public async Task<AnalysisRecordId> Handle(StartAnalysisCommand request, CancellationToken cancellationToken)
    {
        var record = new AnalysisRecord(request.Name, request.Parameters);
        await repo.AddAsync(record, cancellationToken);
        return record.Id;
    }
}
