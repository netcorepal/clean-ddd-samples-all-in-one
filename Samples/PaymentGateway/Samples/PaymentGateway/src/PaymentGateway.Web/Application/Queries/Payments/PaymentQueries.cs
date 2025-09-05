using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.AggregatesModel.OrderAggregate;
using PaymentGateway.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Web.Application.Queries.Payments;

public record GetPaymentQuery(PaymentId PaymentId) : IQuery<PaymentDto>;

public class GetPaymentQueryValidator : AbstractValidator<GetPaymentQuery>
{
    public GetPaymentQueryValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
    }
}

public class GetPaymentQueryHandler(ApplicationDbContext context) : IQueryHandler<GetPaymentQuery, PaymentDto>
{
    public async Task<PaymentDto> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
    {
        var p = await context.Payments
            .Where(x => x.Id == request.PaymentId)
            .Select(x => new PaymentDto(x.Id, x.OrderId, x.Amount, x.Currency, x.Channel, x.Status, x.ProviderTransactionId))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new KnownException($"Payment not found, PaymentId = {request.PaymentId}");
        return p;
    }
}

public record ListPaymentsByOrderQuery(OrderId OrderId, int PageIndex = 1, int PageSize = 20) : IQuery<PagedData<PaymentListItemDto>>;

public class ListPaymentsByOrderQueryValidator : AbstractValidator<ListPaymentsByOrderQuery>
{
    public ListPaymentsByOrderQueryValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.PageIndex).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0).LessThanOrEqualTo(100);
    }
}

public class ListPaymentsByOrderQueryHandler(ApplicationDbContext context) : IQueryHandler<ListPaymentsByOrderQuery, PagedData<PaymentListItemDto>>
{
    public Task<PagedData<PaymentListItemDto>> Handle(ListPaymentsByOrderQuery request, CancellationToken cancellationToken)
    {
        return context.Payments
            .Where(x => x.OrderId == request.OrderId)
            .OrderByDescending(x => x.CreatedTime)
            .Select(x => new PaymentListItemDto(x.Id, x.Amount, x.Currency, x.Channel, x.Status, x.CreatedTime))
            .ToPagedDataAsync(request.PageIndex, request.PageSize, cancellationToken: cancellationToken);
    }
}

public record PaymentDto(PaymentId Id, OrderId OrderId, decimal Amount, string Currency, PaymentChannel Channel, PaymentStatus Status, string? ProviderTransactionId);
public record PaymentListItemDto(PaymentId Id, decimal Amount, string Currency, PaymentChannel Channel, PaymentStatus Status, DateTimeOffset CreatedTime);
