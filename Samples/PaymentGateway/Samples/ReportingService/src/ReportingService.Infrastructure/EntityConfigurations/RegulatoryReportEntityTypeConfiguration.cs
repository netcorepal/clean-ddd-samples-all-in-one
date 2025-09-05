using ReportingService.Domain.AggregatesModel.RegulatoryReportAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ReportingService.Infrastructure.EntityConfigurations;

internal class RegulatoryReportEntityTypeConfiguration : IEntityTypeConfiguration<RegulatoryReport>
{
    public void Configure(EntityTypeBuilder<RegulatoryReport> builder)
    {
        builder.ToTable("regulatory_report");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseSnowFlakeValueGenerator().HasComment("监管报告ID");
        builder.Property(x => x.Category).IsRequired().HasMaxLength(100).HasComment("类别");
        builder.Property(x => x.Period).IsRequired().HasMaxLength(50).HasComment("期间");
        builder.Property(x => x.Payload).IsRequired().HasMaxLength(4000).HasComment("负载");
        builder.Property(x => x.Submitted).IsRequired().HasComment("是否已提交");
    }
}
