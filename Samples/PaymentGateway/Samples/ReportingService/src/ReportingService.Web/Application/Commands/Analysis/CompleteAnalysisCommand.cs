using ReportingService.Domain.AggregatesModel.AnalysisAggregate;
using ReportingService.Infrastructure.Repositories;

namespace ReportingService.Web.Application.Commands.Analysis;

public record CompleteAnalysisCommand(AnalysisRecordId AnalysisId, string Result) : ICommand;

public class CompleteAnalysisCommandValidator : AbstractValidator<CompleteAnalysisCommand>
{
    public CompleteAnalysisCommandValidator()
    {
        RuleFor(x => x.AnalysisId).NotEmpty();
        RuleFor(x => x.Result).NotEmpty().MaximumLength(4000);
    }
}

public class CompleteAnalysisCommandHandler(IAnalysisRecordRepository repo)
    : ICommandHandler<CompleteAnalysisCommand>
{
    public async Task Handle(CompleteAnalysisCommand request, CancellationToken cancellationToken)
    {
        var entity = await repo.GetAsync(request.AnalysisId, cancellationToken)
                     ?? throw new KnownException($"未找到分析记录：{request.AnalysisId}");
        entity.Complete(request.Result);
        await repo.UpdateAsync(entity, cancellationToken);
    }
}
