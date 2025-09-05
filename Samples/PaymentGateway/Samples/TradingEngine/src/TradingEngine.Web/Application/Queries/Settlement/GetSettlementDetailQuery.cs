using TradingEngine.Domain.AggregatesModel.SettlementAggregate;
using Microsoft.EntityFrameworkCore;

namespace TradingEngine.Web.Application.Queries.Settlement;

public record GetSettlementDetailQuery(SettlementId SettlementId) : IQuery<SettlementDetailDto>;

public record SettlementDetailDto(
    SettlementId Id,
    string UserId,
    SettlementType SettlementType,
    decimal TotalAmount,
    DateTimeOffset SettlementDate,
    SettlementStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ProcessedAt,
    DateTimeOffset? CompletedAt,
    string? FailureReason,
    List<SettlementItemDto> Items
);

public record SettlementItemDto(
    string ReferenceId,
    string Symbol,
    decimal Quantity,
    decimal Price,
    decimal Amount,
    string Description,
    DateTimeOffset CreatedAt
);

public class GetSettlementDetailQueryValidator : AbstractValidator<GetSettlementDetailQuery>
{
    public GetSettlementDetailQueryValidator()
    {
        RuleFor(x => x.SettlementId)
            .NotNull()
            .WithMessage("结算ID不能为空");
    }
}

public class GetSettlementDetailQueryHandler : IQueryHandler<GetSettlementDetailQuery, SettlementDetailDto>
{
    private readonly ApplicationDbContext _context;

    public GetSettlementDetailQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SettlementDetailDto> Handle(GetSettlementDetailQuery request, CancellationToken cancellationToken)
    {
        var settlement = await _context.Set<Domain.AggregatesModel.SettlementAggregate.Settlement>()
            .Where(s => s.Id == request.SettlementId)
            .Select(s => new SettlementDetailDto(
                s.Id,
                s.UserId,
                s.SettlementType,
                s.TotalAmount,
                s.SettlementDate,
                s.Status,
                s.CreatedAt,
                s.ProcessedAt,
                s.CompletedAt,
                s.FailureReason,
                s.Items.Select(i => new SettlementItemDto(
                    i.ReferenceId,
                    i.Symbol,
                    i.Quantity,
                    i.Price,
                    i.Amount,
                    i.Description,
                    i.CreatedAt
                )).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (settlement == null)
        {
            throw new KnownException("结算记录不存在");
        }

        return settlement;
    }
}
