using RiskControl.Domain.AggregatesModel.FraudAggregate;
using RiskControl.Domain.AggregatesModel.CreditAggregate;
using RiskControl.Domain.AggregatesModel.ComplianceAggregate;

namespace RiskControl.Web.Application.Queries;

public class RiskQueries(ApplicationDbContext db)
{
    public async Task<FraudCheck?> GetFraudCheck(FraudCheckId id, CancellationToken ct)
        => await db.FraudChecks.FindAsync(new object[] { id }, ct);

    public async Task<CreditAssessment?> GetCreditAssessment(CreditAssessmentId id, CancellationToken ct)
        => await db.CreditAssessments.FindAsync(new object[] { id }, ct);

    public async Task<ComplianceAlert?> GetComplianceAlert(ComplianceAlertId id, CancellationToken ct)
        => await db.ComplianceAlerts.FindAsync(new object[] { id }, ct);
}
