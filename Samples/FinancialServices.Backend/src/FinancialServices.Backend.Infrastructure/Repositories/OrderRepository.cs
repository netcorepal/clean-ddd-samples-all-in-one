using NetCorePal.Extensions.Repository.EntityFrameworkCore;
using FinancialServices.Backend.Domain.AggregatesModel.OrderAggregate;
using NetCorePal.Extensions.Repository;

namespace FinancialServices.Backend.Infrastructure.Repositories;

public interface IOrderRepository : IRepository<Order, OrderId>
{
}

public class OrderRepository(ApplicationDbContext context) : RepositoryBase<Order, OrderId, ApplicationDbContext>(context), IOrderRepository
{
}

