using TradingEngine.Domain.AggregatesModel.TradeAggregate;
using Microsoft.EntityFrameworkCore;

namespace TradingEngine.Web.Application.Queries.Trade;

public record GetTradesByUserQuery(string UserId, int PageIndex = 1, int PageSize = 20, TradeStatus? Status = null, string? Symbol = null) : IQuery<PagedData<TradeListDto>>;

public record TradeListDto(
    TradeId Id,
    string Symbol,
    TradeType TradeType,
    decimal Quantity,
    decimal Price,
    decimal ExecutedQuantity,
    TradeStatus Status,
    DateTimeOffset CreatedAt
);

public class GetTradesByUserQueryValidator : AbstractValidator<GetTradesByUserQuery>
{
    public GetTradesByUserQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("用户ID不能为空");

        RuleFor(x => x.PageIndex)
            .GreaterThan(0)
            .WithMessage("页码必须大于0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("页大小必须在1-100之间");
    }
}

public class GetTradesByUserQueryHandler : IQueryHandler<GetTradesByUserQuery, PagedData<TradeListDto>>
{
    private readonly ApplicationDbContext _context;

    public GetTradesByUserQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedData<TradeListDto>> Handle(GetTradesByUserQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Set<Domain.AggregatesModel.TradeAggregate.Trade>()
            .Where(t => t.UserId == request.UserId)
            .WhereIf(request.Status.HasValue, t => t.Status == request.Status)
            .WhereIf(!string.IsNullOrEmpty(request.Symbol), t => t.Symbol == request.Symbol)
            .OrderByDescending(t => t.CreatedAt);

        return await query
            .Select(t => new TradeListDto(
                t.Id,
                t.Symbol,
                t.TradeType,
                t.Quantity,
                t.Price,
                t.ExecutedQuantity,
                t.Status,
                t.CreatedAt
            ))
            .ToPagedDataAsync(request.PageIndex, request.PageSize, false, cancellationToken);
    }
}
