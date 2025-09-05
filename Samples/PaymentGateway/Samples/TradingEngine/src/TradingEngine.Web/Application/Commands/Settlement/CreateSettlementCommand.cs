using TradingEngine.Domain.AggregatesModel.SettlementAggregate;
using TradingEngine.Infrastructure.Repositories;

namespace TradingEngine.Web.Application.Commands.Settlement;

public record CreateSettlementCommand(string UserId, SettlementType SettlementType, DateTimeOffset SettlementDate) : ICommand<SettlementId>;

public class CreateSettlementCommandValidator : AbstractValidator<CreateSettlementCommand>
{
    public CreateSettlementCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("用户ID不能为空");

        RuleFor(x => x.SettlementType)
            .IsInEnum()
            .WithMessage("结算类型无效");

        RuleFor(x => x.SettlementDate)
            .NotEqual(default(DateTimeOffset))
            .WithMessage("结算日期不能为空");
    }
}

public class CreateSettlementCommandHandler : ICommandHandler<CreateSettlementCommand, SettlementId>
{
    private readonly ISettlementRepository _settlementRepository;

    public CreateSettlementCommandHandler(ISettlementRepository settlementRepository)
    {
        _settlementRepository = settlementRepository;
    }

    public async Task<SettlementId> Handle(CreateSettlementCommand request, CancellationToken cancellationToken)
    {
        var settlement = new Domain.AggregatesModel.SettlementAggregate.Settlement(
            request.UserId,
            request.SettlementType,
            0, // 初始金额为0，后续添加项目时会重新计算
            request.SettlementDate);

        await _settlementRepository.AddAsync(settlement, cancellationToken);
        
        return settlement.Id;
    }
}
