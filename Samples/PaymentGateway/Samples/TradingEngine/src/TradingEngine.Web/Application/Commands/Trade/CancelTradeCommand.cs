using TradingEngine.Domain.AggregatesModel.TradeAggregate;
using TradingEngine.Infrastructure.Repositories;

namespace TradingEngine.Web.Application.Commands.Trade;

public record CancelTradeCommand(TradeId TradeId) : ICommand;

public class CancelTradeCommandValidator : AbstractValidator<CancelTradeCommand>
{
    public CancelTradeCommandValidator()
    {
        RuleFor(x => x.TradeId)
            .NotNull()
            .WithMessage("交易ID不能为空");
    }
}

public class CancelTradeCommandHandler : ICommandHandler<CancelTradeCommand>
{
    private readonly ITradeRepository _tradeRepository;

    public CancelTradeCommandHandler(ITradeRepository tradeRepository)
    {
        _tradeRepository = tradeRepository;
    }

    public async Task Handle(CancelTradeCommand request, CancellationToken cancellationToken)
    {
        var trade = await _tradeRepository.GetAsync(request.TradeId, cancellationToken);
        if (trade == null)
        {
            throw new KnownException("交易不存在");
        }

        trade.Cancel();
        
        await _tradeRepository.UpdateAsync(trade, cancellationToken);
    }
}
