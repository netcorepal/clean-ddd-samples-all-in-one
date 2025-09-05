using TradingEngine.Domain.AggregatesModel.SettlementAggregate;
using Microsoft.EntityFrameworkCore;

namespace TradingEngine.Web.Application.Queries.Settlement;

public record GetSettlementsByUserQuery(string UserId, int PageIndex = 1, int PageSize = 20, SettlementStatus? Status = null) : IQuery<PagedData<SettlementListDto>>;

public record SettlementListDto(
    SettlementId Id,
    SettlementType SettlementType,
    decimal TotalAmount,
    DateTimeOffset SettlementDate,
    SettlementStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? CompletedAt
);

public class GetSettlementsByUserQueryValidator : AbstractValidator<GetSettlementsByUserQuery>
{
    public GetSettlementsByUserQueryValidator()
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

public class GetSettlementsByUserQueryHandler : IQueryHandler<GetSettlementsByUserQuery, PagedData<SettlementListDto>>
{
    private readonly ApplicationDbContext _context;

    public GetSettlementsByUserQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedData<SettlementListDto>> Handle(GetSettlementsByUserQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Set<Domain.AggregatesModel.SettlementAggregate.Settlement>()
            .Where(s => s.UserId == request.UserId)
            .WhereIf(request.Status.HasValue, s => s.Status == request.Status)
            .OrderByDescending(s => s.SettlementDate);

        return await query
            .Select(s => new SettlementListDto(
                s.Id,
                s.SettlementType,
                s.TotalAmount,
                s.SettlementDate,
                s.Status,
                s.CreatedAt,
                s.CompletedAt
            ))
            .ToPagedDataAsync(request.PageIndex, request.PageSize, false, cancellationToken);
    }
}
