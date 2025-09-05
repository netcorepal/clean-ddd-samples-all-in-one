using TradingEngine.Domain.AggregatesModel.TradeAggregate;
using TradingEngine.Infrastructure.Repositories;

namespace TradingEngine.Web.Application.Commands.Trade;

public record ExecuteTradeCommand(TradeId TradeId, decimal ExecutedQuantity, decimal ExecutedPrice) : ICommand;

public class ExecuteTradeCommandValidator : AbstractValidator<ExecuteTradeCommand>
{
    public ExecuteTradeCommandValidator()
    {
        RuleFor(x => x.TradeId)
            .NotNull()
            .WithMessage("交易ID不能为空");

        RuleFor(x => x.ExecutedQuantity)
            .GreaterThan(0)
            .WithMessage("执行数量必须大于0");

        RuleFor(x => x.ExecutedPrice)
            .GreaterThan(0)
            .WithMessage("执行价格必须大于0");
    }
}

public class ExecuteTradeCommandHandler : ICommandHandler<ExecuteTradeCommand>
{
    private readonly ITradeRepository _tradeRepository;

    public ExecuteTradeCommandHandler(ITradeRepository tradeRepository)
    {
        _tradeRepository = tradeRepository;
    }

    public async Task Handle(ExecuteTradeCommand request, CancellationToken cancellationToken)
    {
        var trade = await _tradeRepository.GetAsync(request.TradeId, cancellationToken);
        if (trade == null)
        {
            throw new KnownException("交易不存在");
        }

        trade.Execute(request.ExecutedQuantity, request.ExecutedPrice);
        
        await _tradeRepository.UpdateAsync(trade, cancellationToken);
    }
}
