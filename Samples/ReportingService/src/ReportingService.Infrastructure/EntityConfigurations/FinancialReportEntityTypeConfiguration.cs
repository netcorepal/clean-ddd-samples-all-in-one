using ReportingService.Domain.AggregatesModel.FinancialReportAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ReportingService.Infrastructure.EntityConfigurations;

internal class FinancialReportEntityTypeConfiguration : IEntityTypeConfiguration<FinancialReport>
{
    public void Configure(EntityTypeBuilder<FinancialReport> builder)
    {
        builder.ToTable("financial_report");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseSnowFlakeValueGenerator().HasComment("财务报表ID");
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200).HasComment("标题");
        builder.Property(x => x.Period).IsRequired().HasMaxLength(50).HasComment("报表期间");
        builder.Property(x => x.Content).IsRequired().HasMaxLength(4000).HasComment("内容");
        builder.Property(x => x.GeneratedAt).IsRequired().HasComment("生成时间");
    }
}
