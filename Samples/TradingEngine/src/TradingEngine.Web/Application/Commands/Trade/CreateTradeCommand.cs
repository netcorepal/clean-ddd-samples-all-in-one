using TradingEngine.Domain.AggregatesModel.TradeAggregate;
using TradingEngine.Domain.AggregatesModel.RiskControlAggregate;
using TradingEngine.Infrastructure.Repositories;

namespace TradingEngine.Web.Application.Commands.Trade;

public record CreateTradeCommand(string Symbol, TradeType TradeType, decimal Quantity, decimal Price, string UserId) : ICommand<TradeId>;

public class CreateTradeCommandValidator : AbstractValidator<CreateTradeCommand>
{
    public CreateTradeCommandValidator()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty()
            .MaximumLength(20)
            .WithMessage("交易标的不能为空且长度不能超过20个字符");

        RuleFor(x => x.TradeType)
            .IsInEnum()
            .WithMessage("交易类型无效");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("交易数量必须大于0");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("交易价格必须大于0");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("用户ID不能为空");
    }
}

public class CreateTradeCommandHandler : ICommandHandler<CreateTradeCommand, TradeId>
{
    private readonly ITradeRepository _tradeRepository;
    private readonly IRiskControlRepository _riskControlRepository;

    public CreateTradeCommandHandler(ITradeRepository tradeRepository, IRiskControlRepository riskControlRepository)
    {
        _tradeRepository = tradeRepository;
        _riskControlRepository = riskControlRepository;
    }

    public async Task<TradeId> Handle(CreateTradeCommand request, CancellationToken cancellationToken)
    {
        // 1. 进行风险评估
        var riskControl = await _riskControlRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (riskControl != null && riskControl.IsActive)
        {
            var riskAssessment = riskControl.AssessTradeRisk(request.Symbol, request.Quantity, request.Price, request.TradeType);
            
            // 如果风险等级过高，拒绝交易
            if (riskAssessment.RiskLevel == RiskLevel.Critical)
            {
                throw new KnownException($"交易风险过高，拒绝执行: {riskAssessment.Description}");
            }

            await _riskControlRepository.UpdateAsync(riskControl, cancellationToken);
        }

        // 2. 创建交易
        var trade = new Domain.AggregatesModel.TradeAggregate.Trade(
            request.Symbol, 
            request.TradeType, 
            request.Quantity, 
            request.Price, 
            request.UserId);

        await _tradeRepository.AddAsync(trade, cancellationToken);
        
        return trade.Id;
    }
}
