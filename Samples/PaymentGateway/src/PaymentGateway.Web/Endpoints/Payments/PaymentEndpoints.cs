using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using PaymentGateway.Web.Application.Commands.Payments;
using PaymentGateway.Web.Application.Queries.Payments;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.AggregatesModel.OrderAggregate;

namespace PaymentGateway.Web.Endpoints.Payments;

public record CreatePaymentRequest(OrderId OrderId, decimal Amount, string Currency, PaymentChannel Channel);
public record CreatePaymentResponse(PaymentId PaymentId);

[AllowAnonymous]
[HttpPost("/api/payments")]
public class CreatePaymentEndpoint(IMediator mediator) : Endpoint<CreatePaymentRequest, ResponseData<CreatePaymentResponse>>
{
    public override async Task HandleAsync(CreatePaymentRequest req, CancellationToken ct)
    {
        var id = await mediator.Send(new CreatePaymentCommand(req.OrderId, req.Amount, req.Currency, req.Channel), ct);
        await Send.OkAsync(new CreatePaymentResponse(id).AsResponseData(), ct);
    }
}

public record MarkPaymentSucceededRequest(PaymentId PaymentId, string ProviderTransactionId);

[AllowAnonymous]
[HttpPost("/api/payments/succeeded")]
public class MarkPaymentSucceededEndpoint(IMediator mediator) : Endpoint<MarkPaymentSucceededRequest, EmptyResponse>
{
    public override async Task HandleAsync(MarkPaymentSucceededRequest req, CancellationToken ct)
    {
        await mediator.Send(new MarkPaymentSucceededCommand(req.PaymentId, req.ProviderTransactionId), ct);
        await Send.NoContentAsync(ct);
    }
}

public record MarkPaymentFailedRequest(PaymentId PaymentId, string Reason);

[AllowAnonymous]
[HttpPost("/api/payments/failed")]
public class MarkPaymentFailedEndpoint(IMediator mediator) : Endpoint<MarkPaymentFailedRequest, EmptyResponse>
{
    public override async Task HandleAsync(MarkPaymentFailedRequest req, CancellationToken ct)
    {
        await mediator.Send(new MarkPaymentFailedCommand(req.PaymentId, req.Reason), ct);
        await Send.NoContentAsync(ct);
    }
}

[AllowAnonymous]
[HttpGet("/api/payments/{paymentId}")]
public class GetPaymentEndpoint(IMediator mediator) : EndpointWithoutRequest<ResponseData<PaymentDto>>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<PaymentId>("paymentId");
        if (id == default)
        {
            throw new KnownException("paymentId is required");
        }
        var dto = await mediator.Send(new GetPaymentQuery(id!), ct);
        await Send.OkAsync(dto.AsResponseData(), ct);
    }
}
