using RiskControl.Domain.AggregatesModel.CreditAggregate;
using RiskControl.Domain.AggregatesModel.OrderAggregate;
using RiskControl.Infrastructure.Repositories;

namespace RiskControl.Web.Application.Commands;

public record RunCreditAssessmentCommand(OrderId OrderId, string CustomerId, decimal Exposure) : ICommand<CreditAssessmentId>;

public class RunCreditAssessmentCommandValidator : AbstractValidator<RunCreditAssessmentCommand>
{
    public RunCreditAssessmentCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Exposure).GreaterThanOrEqualTo(0);
    }
}

public class RunCreditAssessmentCommandHandler(ICreditAssessmentRepository repository) : ICommandHandler<RunCreditAssessmentCommand, CreditAssessmentId>
{
    public async Task<CreditAssessmentId> Handle(RunCreditAssessmentCommand request, CancellationToken cancellationToken)
    {
        var assessment = new CreditAssessment(request.OrderId, request.CustomerId, request.Exposure);
        // simple mock scoring logic
        var baseScore = 650;
        if (request.Exposure < 5000) baseScore += 50;
        if (request.Exposure > 20000) baseScore -= 80;
        assessment.Complete(baseScore);
        await repository.AddAsync(assessment, cancellationToken);
        return assessment.Id;
    }
}
