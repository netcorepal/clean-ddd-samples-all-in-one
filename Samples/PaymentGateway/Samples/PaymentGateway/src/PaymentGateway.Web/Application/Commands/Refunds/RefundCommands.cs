using PaymentGateway.Domain.AggregatesModel.RefundAggregate;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Infrastructure.Repositories;

namespace PaymentGateway.Web.Application.Commands.Refunds;

public record RequestRefundCommand(PaymentId PaymentId, decimal Amount, string Reason) : ICommand<RefundId>;

public class RequestRefundCommandValidator : AbstractValidator<RequestRefundCommand>
{
    public RequestRefundCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(200);
    }
}

public class RequestRefundCommandHandler(IRefundRepository refundRepository) : ICommandHandler<RequestRefundCommand, RefundId>
{
    public async Task<RefundId> Handle(RequestRefundCommand request, CancellationToken cancellationToken)
    {
        var refund = new Refund(request.PaymentId, request.Amount, request.Reason);
        await refundRepository.AddAsync(refund, cancellationToken);
        return refund.Id;
    }
}

public record ConfirmRefundSucceededCommand(RefundId RefundId, string ProviderRefundId) : ICommand;

public class ConfirmRefundSucceededCommandValidator : AbstractValidator<ConfirmRefundSucceededCommand>
{
    public ConfirmRefundSucceededCommandValidator()
    {
        RuleFor(x => x.RefundId).NotEmpty();
        RuleFor(x => x.ProviderRefundId).NotEmpty();
    }
}

public class ConfirmRefundSucceededCommandHandler(IRefundRepository refundRepository) : ICommandHandler<ConfirmRefundSucceededCommand>
{
    public async Task Handle(ConfirmRefundSucceededCommand request, CancellationToken cancellationToken)
    {
        var refund = await refundRepository.GetAsync(request.RefundId, cancellationToken)
                     ?? throw new KnownException($"Refund not found, RefundId = {request.RefundId}");
        refund.MarkSucceeded(request.ProviderRefundId);
        await refundRepository.UpdateAsync(refund, cancellationToken);
    }
}

public record ConfirmRefundFailedCommand(RefundId RefundId, string Reason) : ICommand;

public class ConfirmRefundFailedCommandValidator : AbstractValidator<ConfirmRefundFailedCommand>
{
    public ConfirmRefundFailedCommandValidator()
    {
        RuleFor(x => x.RefundId).NotEmpty();
        RuleFor(x => x.Reason).NotEmpty();
    }
}

public class ConfirmRefundFailedCommandHandler(IRefundRepository refundRepository) : ICommandHandler<ConfirmRefundFailedCommand>
{
    public async Task Handle(ConfirmRefundFailedCommand request, CancellationToken cancellationToken)
    {
        var refund = await refundRepository.GetAsync(request.RefundId, cancellationToken)
                     ?? throw new KnownException($"Refund not found, RefundId = {request.RefundId}");
        refund.MarkFailed(request.Reason);
        await refundRepository.UpdateAsync(refund, cancellationToken);
    }
}
