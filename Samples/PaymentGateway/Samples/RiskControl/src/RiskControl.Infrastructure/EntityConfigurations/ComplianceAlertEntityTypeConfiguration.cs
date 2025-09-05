using RiskControl.Domain.AggregatesModel.ComplianceAggregate;
using RiskControl.Domain.AggregatesModel.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RiskControl.Infrastructure.EntityConfigurations;

public class ComplianceAlertEntityTypeConfiguration : IEntityTypeConfiguration<ComplianceAlert>
{
    public void Configure(EntityTypeBuilder<ComplianceAlert> builder)
    {
        builder.ToTable("ComplianceAlerts");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .UseGuidVersion7ValueGenerator()
            .HasComment("合规告警ID");

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasComment("订单ID");

        builder.Property(x => x.RuleCode)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("规则编码");

        builder.Property(x => x.Detail)
            .IsRequired()
            .HasMaxLength(500)
            .HasComment("告警详情");

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("状态");

        builder.Property(x => x.Resolution)
            .HasMaxLength(500)
            .HasComment("处理说明");

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasComment("创建时间");

        builder.Property(x => x.ClosedAt)
            .HasComment("关闭时间");

        builder.HasIndex(x => new { x.RuleCode, x.Status, x.CreatedAt });
    }
}
