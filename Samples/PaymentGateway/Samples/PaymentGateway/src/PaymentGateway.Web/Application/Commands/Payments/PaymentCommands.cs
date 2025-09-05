using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.AggregatesModel.OrderAggregate;
using PaymentGateway.Infrastructure.Repositories;

namespace PaymentGateway.Web.Application.Commands.Payments;

public record CreatePaymentCommand(OrderId OrderId, decimal Amount, string Currency, PaymentChannel Channel) : ICommand<PaymentId>;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Currency).NotEmpty().MaximumLength(10);
    }
}

public class CreatePaymentCommandHandler(IPaymentRepository paymentRepository) : ICommandHandler<CreatePaymentCommand, PaymentId>
{
    public async Task<PaymentId> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = new Payment(request.OrderId, request.Amount, request.Currency, request.Channel);
        await paymentRepository.AddAsync(payment, cancellationToken);
        return payment.Id;
    }
}

public record MarkPaymentSucceededCommand(PaymentId PaymentId, string ProviderTransactionId) : ICommand;

public class MarkPaymentSucceededCommandValidator : AbstractValidator<MarkPaymentSucceededCommand>
{
    public MarkPaymentSucceededCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
        RuleFor(x => x.ProviderTransactionId).NotEmpty();
    }
}

public class MarkPaymentSucceededCommandHandler(IPaymentRepository paymentRepository) : ICommandHandler<MarkPaymentSucceededCommand>
{
    public async Task Handle(MarkPaymentSucceededCommand request, CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetAsync(request.PaymentId, cancellationToken)
                      ?? throw new KnownException($"Payment not found, PaymentId = {request.PaymentId}");
        payment.MarkSucceeded(request.ProviderTransactionId);
        await paymentRepository.UpdateAsync(payment, cancellationToken);
    }
}

public record MarkPaymentFailedCommand(PaymentId PaymentId, string Reason) : ICommand;

public class MarkPaymentFailedCommandValidator : AbstractValidator<MarkPaymentFailedCommand>
{
    public MarkPaymentFailedCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
        RuleFor(x => x.Reason).NotEmpty();
    }
}

public class MarkPaymentFailedCommandHandler(IPaymentRepository paymentRepository) : ICommandHandler<MarkPaymentFailedCommand>
{
    public async Task Handle(MarkPaymentFailedCommand request, CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetAsync(request.PaymentId, cancellationToken)
                      ?? throw new KnownException($"Payment not found, PaymentId = {request.PaymentId}");
        payment.MarkFailed(request.Reason);
        await paymentRepository.UpdateAsync(payment, cancellationToken);
    }
}
