using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using PaymentGateway.Web.Application.Commands.Refunds;
using PaymentGateway.Domain.AggregatesModel.RefundAggregate;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;

namespace PaymentGateway.Web.Endpoints.Refunds;

public record RequestRefundRequest(PaymentId PaymentId, decimal Amount, string Reason);
public record RequestRefundResponse(RefundId RefundId);

[AllowAnonymous]
[HttpPost("/api/refunds")]
public class RequestRefundEndpoint(IMediator mediator) : Endpoint<RequestRefundRequest, ResponseData<RequestRefundResponse>>
{
    public override async Task HandleAsync(RequestRefundRequest req, CancellationToken ct)
    {
        var id = await mediator.Send(new RequestRefundCommand(req.PaymentId, req.Amount, req.Reason), ct);
        await Send.OkAsync(new RequestRefundResponse(id).AsResponseData(), ct);
    }
}

public record ConfirmRefundSucceededRequest(RefundId RefundId, string ProviderRefundId);

[AllowAnonymous]
[HttpPost("/api/refunds/succeeded")]
public class ConfirmRefundSucceededEndpoint(IMediator mediator) : Endpoint<ConfirmRefundSucceededRequest, EmptyResponse>
{
    public override async Task HandleAsync(ConfirmRefundSucceededRequest req, CancellationToken ct)
    {
        await mediator.Send(new ConfirmRefundSucceededCommand(req.RefundId, req.ProviderRefundId), ct);
        await Send.NoContentAsync(ct);
    }
}

public record ConfirmRefundFailedRequest(RefundId RefundId, string Reason);

[AllowAnonymous]
[HttpPost("/api/refunds/failed")]
public class ConfirmRefundFailedEndpoint(IMediator mediator) : Endpoint<ConfirmRefundFailedRequest, EmptyResponse>
{
    public override async Task HandleAsync(ConfirmRefundFailedRequest req, CancellationToken ct)
    {
        await mediator.Send(new ConfirmRefundFailedCommand(req.RefundId, req.Reason), ct);
        await Send.NoContentAsync(ct);
    }
}
