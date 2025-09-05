using ReportingService.Domain.AggregatesModel.AnalysisAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ReportingService.Infrastructure.EntityConfigurations;

internal class AnalysisRecordEntityTypeConfiguration : IEntityTypeConfiguration<AnalysisRecord>
{
    public void Configure(EntityTypeBuilder<AnalysisRecord> builder)
    {
        builder.ToTable("analysis_record");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseSnowFlakeValueGenerator().HasComment("分析ID");
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200).HasComment("分析名称");
        builder.Property(x => x.Parameters).IsRequired().HasMaxLength(2000).HasComment("参数");
        builder.Property(x => x.Status).IsRequired().HasMaxLength(50).HasComment("状态");
        builder.Property(x => x.StartedAt).IsRequired().HasComment("开始时间");
        builder.Property(x => x.CompletedAt).HasComment("完成时间");
        builder.Property(x => x.Result).HasMaxLength(4000).HasComment("结果");
    }
}
