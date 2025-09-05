using TradingEngine.Domain.AggregatesModel.RiskControlAggregate;
using TradingEngine.Infrastructure.Repositories;

namespace TradingEngine.Web.Application.Commands.RiskControl;

public record CreateRiskControlCommand(string UserId, decimal TotalPositionLimit, decimal DailyLossLimit) : ICommand<RiskControlId>;

public class CreateRiskControlCommandValidator : AbstractValidator<CreateRiskControlCommand>
{
    public CreateRiskControlCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("用户ID不能为空");

        RuleFor(x => x.TotalPositionLimit)
            .GreaterThan(0)
            .WithMessage("总持仓限制必须大于0");

        RuleFor(x => x.DailyLossLimit)
            .GreaterThan(0)
            .WithMessage("日损失限制必须大于0");
    }
}

public class CreateRiskControlCommandHandler : ICommandHandler<CreateRiskControlCommand, RiskControlId>
{
    private readonly IRiskControlRepository _riskControlRepository;

    public CreateRiskControlCommandHandler(IRiskControlRepository riskControlRepository)
    {
        _riskControlRepository = riskControlRepository;
    }

    public async Task<RiskControlId> Handle(CreateRiskControlCommand request, CancellationToken cancellationToken)
    {
        // 检查用户是否已有风险控制配置
        var existingRiskControl = await _riskControlRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (existingRiskControl != null)
        {
            throw new KnownException("用户已存在风险控制配置");
        }

        var riskControl = new Domain.AggregatesModel.RiskControlAggregate.RiskControl(
            request.UserId,
            request.TotalPositionLimit,
            request.DailyLossLimit);

        await _riskControlRepository.AddAsync(riskControl, cancellationToken);
        
        return riskControl.Id;
    }
}
