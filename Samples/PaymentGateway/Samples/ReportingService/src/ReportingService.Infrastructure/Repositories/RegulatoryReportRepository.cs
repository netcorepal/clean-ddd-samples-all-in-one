using NetCorePal.Extensions.Repository.EntityFrameworkCore;
using NetCorePal.Extensions.Repository;
using ReportingService.Domain.AggregatesModel.RegulatoryReportAggregate;

namespace ReportingService.Infrastructure.Repositories;

public interface IRegulatoryReportRepository : IRepository<RegulatoryReport, RegulatoryReportId>
{
}

public class RegulatoryReportRepository(ApplicationDbContext context)
    : RepositoryBase<RegulatoryReport, RegulatoryReportId, ApplicationDbContext>(context), IRegulatoryReportRepository
{
}
