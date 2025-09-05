using NetCorePal.Extensions.Repository.EntityFrameworkCore;
using NetCorePal.Extensions.Repository;
using ReportingService.Domain.AggregatesModel.FinancialReportAggregate;

namespace ReportingService.Infrastructure.Repositories;

public interface IFinancialReportRepository : IRepository<FinancialReport, FinancialReportId>
{
}

public class FinancialReportRepository(ApplicationDbContext context)
    : RepositoryBase<FinancialReport, FinancialReportId, ApplicationDbContext>(context), IFinancialReportRepository
{
}
