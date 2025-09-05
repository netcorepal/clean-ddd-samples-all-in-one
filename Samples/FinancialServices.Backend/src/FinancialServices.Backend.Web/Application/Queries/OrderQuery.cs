using FinancialServices.Backend.Domain;
using FinancialServices.Backend.Domain.AggregatesModel.OrderAggregate;
using FinancialServices.Backend.Infrastructure;
using System.Threading;

namespace FinancialServices.Backend.Web.Application.Queries
{
    public class OrderQuery(ApplicationDbContext applicationDbContext)
    {
        public async Task<Order?> QueryOrder(OrderId orderId, CancellationToken cancellationToken)
        {
            return await applicationDbContext.Orders.FindAsync(new object[] { orderId }, cancellationToken);
        }
    }
}
