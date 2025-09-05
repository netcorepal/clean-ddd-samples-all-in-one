using RiskControl.Domain.AggregatesModel.OrderAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RiskControl.Domain.AggregatesModel.DeliverAggregate;
using RiskControl.Domain.AggregatesModel.FraudAggregate;
using RiskControl.Domain.AggregatesModel.CreditAggregate;
using RiskControl.Domain.AggregatesModel.ComplianceAggregate;

namespace RiskControl.Infrastructure;

public partial class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator)
    : AppDbContextBase(options, mediator)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }


    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        ConfigureStronglyTypedIdValueConverter(configurationBuilder);
        base.ConfigureConventions(configurationBuilder);
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<DeliverRecord> DeliverRecords => Set<DeliverRecord>();
    public DbSet<FraudCheck> FraudChecks => Set<FraudCheck>();
    public DbSet<CreditAssessment> CreditAssessments => Set<CreditAssessment>();
    public DbSet<ComplianceAlert> ComplianceAlerts => Set<ComplianceAlert>();
}
