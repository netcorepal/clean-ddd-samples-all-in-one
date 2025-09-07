using MediatR;
using ReportingService.Domain.AggregatesModel.FinancialReportAggregate;
using ReportingService.Domain.AggregatesModel.RegulatoryReportAggregate;
using ReportingService.Domain.AggregatesModel.AnalysisAggregate;

namespace ReportingService.Infrastructure;

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

    public DbSet<FinancialReport> FinancialReports => Set<FinancialReport>();
    public DbSet<RegulatoryReport> RegulatoryReports => Set<RegulatoryReport>();
    public DbSet<AnalysisRecord> AnalysisRecords => Set<AnalysisRecord>();
}
