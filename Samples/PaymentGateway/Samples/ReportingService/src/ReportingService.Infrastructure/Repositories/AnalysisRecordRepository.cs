using NetCorePal.Extensions.Repository.EntityFrameworkCore;
using NetCorePal.Extensions.Repository;
using ReportingService.Domain.AggregatesModel.AnalysisAggregate;

namespace ReportingService.Infrastructure.Repositories;

public interface IAnalysisRecordRepository : IRepository<AnalysisRecord, AnalysisRecordId>
{
}

public class AnalysisRecordRepository(ApplicationDbContext context)
    : RepositoryBase<AnalysisRecord, AnalysisRecordId, ApplicationDbContext>(context), IAnalysisRecordRepository
{
}
