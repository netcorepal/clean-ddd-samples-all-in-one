using RiskControl.Domain;
using RiskControl.Domain.AggregatesModel.OrderAggregate;
using RiskControl.Infrastructure;
using System.Threading;

namespace RiskControl.Web.Application.Queries
{
    public class OrderQuery(ApplicationDbContext applicationDbContext)
    {
        public async Task<Order?> QueryOrder(OrderId orderId, CancellationToken cancellationToken)
        {
            return await applicationDbContext.Orders.FindAsync(new object[] { orderId }, cancellationToken);
        }
    }
}
