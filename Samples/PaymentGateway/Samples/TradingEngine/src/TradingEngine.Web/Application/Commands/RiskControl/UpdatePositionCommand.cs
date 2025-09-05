using TradingEngine.Domain.AggregatesModel.RiskControlAggregate;
using TradingEngine.Infrastructure.Repositories;

namespace TradingEngine.Web.Application.Commands.RiskControl;

public record UpdatePositionCommand(string UserId, decimal PositionChange) : ICommand;

public class UpdatePositionCommandValidator : AbstractValidator<UpdatePositionCommand>
{
    public UpdatePositionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("用户ID不能为空");

        RuleFor(x => x.PositionChange)
            .NotEqual(0)
            .WithMessage("持仓变化不能为0");
    }
}

public class UpdatePositionCommandHandler : ICommandHandler<UpdatePositionCommand>
{
    private readonly IRiskControlRepository _riskControlRepository;

    public UpdatePositionCommandHandler(IRiskControlRepository riskControlRepository)
    {
        _riskControlRepository = riskControlRepository;
    }

    public async Task Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
    {
        var riskControl = await _riskControlRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (riskControl == null)
        {
            throw new KnownException("用户风险控制配置不存在");
        }

        riskControl.UpdatePosition(request.PositionChange);
        
        await _riskControlRepository.UpdateAsync(riskControl, cancellationToken);
    }
}
