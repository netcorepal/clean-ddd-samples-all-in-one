using RiskControl.Domain.AggregatesModel.ComplianceAggregate;

namespace RiskControl.Infrastructure.Repositories;

public interface IComplianceAlertRepository : IRepository<ComplianceAlert, ComplianceAlertId>
{
}

public class ComplianceAlertRepository(ApplicationDbContext context) : RepositoryBase<ComplianceAlert, ComplianceAlertId, ApplicationDbContext>(context), IComplianceAlertRepository
{
}
