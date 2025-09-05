using TradingEngine.Domain.AggregatesModel.RiskControlAggregate;
using TradingEngine.Domain.AggregatesModel.TradeAggregate;
using Microsoft.EntityFrameworkCore;

namespace TradingEngine.Web.Application.Queries.RiskControl;

public record GetRiskControlByUserQuery(string UserId) : IQuery<RiskControlDto>;

public record RiskControlDto(
    RiskControlId Id,
    string UserId,
    decimal TotalPositionLimit,
    decimal DailyLossLimit,
    decimal CurrentPosition,
    decimal DailyLoss,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset? LastAssessmentAt,
    List<RiskAssessmentDto> RiskAssessments
);

public record RiskAssessmentDto(
    string Symbol,
    decimal Quantity,
    decimal Price,
    TradeType TradeType,
    RiskLevel RiskLevel,
    List<RiskType> RiskTypes,
    string Description,
    DateTimeOffset AssessedAt
);

public class GetRiskControlByUserQueryValidator : AbstractValidator<GetRiskControlByUserQuery>
{
    public GetRiskControlByUserQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("用户ID不能为空");
    }
}

public class GetRiskControlByUserQueryHandler : IQueryHandler<GetRiskControlByUserQuery, RiskControlDto>
{
    private readonly ApplicationDbContext _context;

    public GetRiskControlByUserQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RiskControlDto> Handle(GetRiskControlByUserQuery request, CancellationToken cancellationToken)
    {
        var riskControl = await _context.Set<Domain.AggregatesModel.RiskControlAggregate.RiskControl>()
            .Where(rc => rc.UserId == request.UserId)
            .Select(rc => new RiskControlDto(
                rc.Id,
                rc.UserId,
                rc.TotalPositionLimit,
                rc.DailyLossLimit,
                rc.CurrentPosition,
                rc.DailyLoss,
                rc.IsActive,
                rc.CreatedAt,
                rc.LastAssessmentAt,
                rc.RiskAssessments.Select(ra => new RiskAssessmentDto(
                    ra.Symbol,
                    ra.Quantity,
                    ra.Price,
                    ra.TradeType,
                    ra.RiskLevel,
                    ra.RiskTypes,
                    ra.Description,
                    ra.AssessedAt
                )).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (riskControl == null)
        {
            throw new KnownException("用户风险控制配置不存在");
        }

        return riskControl;
    }
}
