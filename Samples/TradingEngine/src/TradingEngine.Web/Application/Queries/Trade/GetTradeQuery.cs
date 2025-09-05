using TradingEngine.Domain.AggregatesModel.TradeAggregate;
using Microsoft.EntityFrameworkCore;

namespace TradingEngine.Web.Application.Queries.Trade;

public record GetTradeQuery(TradeId TradeId) : IQuery<TradeDto>;

public record TradeDto(
    TradeId Id,
    string Symbol,
    TradeType TradeType,
    decimal Quantity,
    decimal Price,
    decimal ExecutedQuantity,
    decimal RemainingQuantity,
    decimal TotalValue,
    TradeStatus Status,
    string UserId,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ExecutedAt,
    string? FailureReason
);

public class GetTradeQueryValidator : AbstractValidator<GetTradeQuery>
{
    public GetTradeQueryValidator()
    {
        RuleFor(x => x.TradeId)
            .NotNull()
            .WithMessage("交易ID不能为空");
    }
}

public class GetTradeQueryHandler : IQueryHandler<GetTradeQuery, TradeDto>
{
    private readonly ApplicationDbContext _context;

    public GetTradeQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TradeDto> Handle(GetTradeQuery request, CancellationToken cancellationToken)
    {
        var trade = await _context.Set<Domain.AggregatesModel.TradeAggregate.Trade>()
            .Where(t => t.Id == request.TradeId)
            .Select(t => new TradeDto(
                t.Id,
                t.Symbol,
                t.TradeType,
                t.Quantity,
                t.Price,
                t.ExecutedQuantity,
                t.RemainingQuantity,
                t.TotalValue,
                t.Status,
                t.UserId,
                t.CreatedAt,
                t.ExecutedAt,
                t.FailureReason
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (trade == null)
        {
            throw new KnownException("交易不存在");
        }

        return trade;
    }
}
