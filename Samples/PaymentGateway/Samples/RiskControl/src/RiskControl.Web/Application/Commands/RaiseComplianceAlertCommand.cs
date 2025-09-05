using RiskControl.Domain.AggregatesModel.ComplianceAggregate;
using RiskControl.Domain.AggregatesModel.OrderAggregate;
using RiskControl.Infrastructure.Repositories;

namespace RiskControl.Web.Application.Commands;

public record RaiseComplianceAlertCommand(OrderId OrderId, string RuleCode, string Detail, string? AutoResolution = null) : ICommand<ComplianceAlertId>;

public class RaiseComplianceAlertCommandValidator : AbstractValidator<RaiseComplianceAlertCommand>
{
    public RaiseComplianceAlertCommandValidator()
    {
        RuleFor(x => x.RuleCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Detail).NotEmpty().MaximumLength(500);
    }
}

public class RaiseComplianceAlertCommandHandler(IComplianceAlertRepository repository) : ICommandHandler<RaiseComplianceAlertCommand, ComplianceAlertId>
{
    public async Task<ComplianceAlertId> Handle(RaiseComplianceAlertCommand request, CancellationToken cancellationToken)
    {
        var alert = new ComplianceAlert(request.OrderId, request.RuleCode, request.Detail);
        if (!string.IsNullOrWhiteSpace(request.AutoResolution))
        {
            alert.Close(request.AutoResolution!);
        }
        await repository.AddAsync(alert, cancellationToken);
        return alert.Id;
    }
}
