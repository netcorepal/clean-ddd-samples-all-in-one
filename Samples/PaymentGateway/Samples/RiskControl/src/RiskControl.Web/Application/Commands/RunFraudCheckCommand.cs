using RiskControl.Domain.AggregatesModel.FraudAggregate;
using RiskControl.Domain.AggregatesModel.OrderAggregate;
using RiskControl.Infrastructure.Repositories;

namespace RiskControl.Web.Application.Commands;

public record RunFraudCheckCommand(OrderId OrderId, string Channel, decimal Amount, string IpAddress) : ICommand<FraudCheckId>;

public class RunFraudCheckCommandValidator : AbstractValidator<RunFraudCheckCommand>
{
    public RunFraudCheckCommandValidator()
    {
        RuleFor(x => x.Channel).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.IpAddress).NotEmpty().MaximumLength(45);
    }
}

public class RunFraudCheckCommandHandler(IFraudCheckRepository repository) : ICommandHandler<RunFraudCheckCommand, FraudCheckId>
{
    public async Task<FraudCheckId> Handle(RunFraudCheckCommand request, CancellationToken cancellationToken)
    {
        var check = new FraudCheck(request.OrderId, request.Channel, request.Amount, request.IpAddress);
        // naive scoring logic for demo
        var score = 0;
        if (request.Amount > 10000) score += 300;
        if (request.Channel.Equals("Unknown", StringComparison.OrdinalIgnoreCase)) score += 200;
        if (request.IpAddress.StartsWith("10.") || request.IpAddress.StartsWith("192.168.")) score += 50;
        string result;
        if (score >= 500)
        {
            result = "Reject";
        }
        else if (score >= 200)
        {
            result = "Review";
        }
        else
        {
            result = "Pass";
        }
        var reasons = result == "Pass" ? string.Empty : "Auto rules triggered";
        check.Complete(score, result, reasons);
        await repository.AddAsync(check, cancellationToken);
        return check.Id;
    }
}
