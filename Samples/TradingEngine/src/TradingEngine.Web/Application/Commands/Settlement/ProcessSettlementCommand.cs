using TradingEngine.Domain.AggregatesModel.SettlementAggregate;
using TradingEngine.Infrastructure.Repositories;

namespace TradingEngine.Web.Application.Commands.Settlement;

public record ProcessSettlementCommand(SettlementId SettlementId) : ICommand;

public class ProcessSettlementCommandValidator : AbstractValidator<ProcessSettlementCommand>
{
    public ProcessSettlementCommandValidator()
    {
        RuleFor(x => x.SettlementId)
            .NotNull()
            .WithMessage("结算ID不能为空");
    }
}

public class ProcessSettlementCommandHandler : ICommandHandler<ProcessSettlementCommand>
{
    private readonly ISettlementRepository _settlementRepository;

    public ProcessSettlementCommandHandler(ISettlementRepository settlementRepository)
    {
        _settlementRepository = settlementRepository;
    }

    public async Task Handle(ProcessSettlementCommand request, CancellationToken cancellationToken)
    {
        var settlement = await _settlementRepository.GetAsync(request.SettlementId, cancellationToken);
        if (settlement == null)
        {
            throw new KnownException("结算记录不存在");
        }

        settlement.StartProcessing();

        try
        {
            // 这里可以添加实际的结算处理逻辑
            // 例如：调用银行API、更新账户余额等
            
            // 模拟处理延迟
            await Task.Delay(100, cancellationToken);
            
            // 处理成功，标记为完成
            settlement.Complete();
        }
        catch (Exception ex)
        {
            settlement.Fail(ex.Message);
            throw;
        }

        await _settlementRepository.UpdateAsync(settlement, cancellationToken);
    }
}
