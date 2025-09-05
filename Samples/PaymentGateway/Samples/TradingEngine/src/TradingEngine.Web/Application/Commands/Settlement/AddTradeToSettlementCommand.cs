using TradingEngine.Domain.AggregatesModel.SettlementAggregate;
using TradingEngine.Domain.AggregatesModel.TradeAggregate;
using TradingEngine.Infrastructure.Repositories;

namespace TradingEngine.Web.Application.Commands.Settlement;

public record AddTradeToSettlementCommand(SettlementId SettlementId, TradeId TradeId, string Symbol, decimal Quantity, decimal Price, TradeType TradeType) : ICommand;

public class AddTradeToSettlementCommandValidator : AbstractValidator<AddTradeToSettlementCommand>
{
    public AddTradeToSettlementCommandValidator()
    {
        RuleFor(x => x.SettlementId)
            .NotNull()
            .WithMessage("结算ID不能为空");

        RuleFor(x => x.TradeId)
            .NotNull()
            .WithMessage("交易ID不能为空");

        RuleFor(x => x.Symbol)
            .NotEmpty()
            .WithMessage("交易标的不能为空");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("数量必须大于0");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("价格必须大于0");

        RuleFor(x => x.TradeType)
            .IsInEnum()
            .WithMessage("交易类型无效");
    }
}

public class AddTradeToSettlementCommandHandler : ICommandHandler<AddTradeToSettlementCommand>
{
    private readonly ISettlementRepository _settlementRepository;

    public AddTradeToSettlementCommandHandler(ISettlementRepository settlementRepository)
    {
        _settlementRepository = settlementRepository;
    }

    public async Task Handle(AddTradeToSettlementCommand request, CancellationToken cancellationToken)
    {
        var settlement = await _settlementRepository.GetAsync(request.SettlementId, cancellationToken);
        if (settlement == null)
        {
            throw new KnownException("结算记录不存在");
        }

        settlement.AddTradeSettlementItem(
            request.TradeId,
            request.Symbol,
            request.Quantity,
            request.Price,
            request.TradeType);

        await _settlementRepository.UpdateAsync(settlement, cancellationToken);
    }
}
