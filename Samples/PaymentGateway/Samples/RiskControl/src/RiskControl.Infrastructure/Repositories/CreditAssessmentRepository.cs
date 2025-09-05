using RiskControl.Domain.AggregatesModel.CreditAggregate;

namespace RiskControl.Infrastructure.Repositories;

public interface ICreditAssessmentRepository : IRepository<CreditAssessment, CreditAssessmentId>
{
}

public class CreditAssessmentRepository(ApplicationDbContext context) : RepositoryBase<CreditAssessment, CreditAssessmentId, ApplicationDbContext>(context), ICreditAssessmentRepository
{
}
