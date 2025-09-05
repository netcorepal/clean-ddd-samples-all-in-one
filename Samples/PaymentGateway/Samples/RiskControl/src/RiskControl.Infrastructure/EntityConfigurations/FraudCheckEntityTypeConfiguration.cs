using RiskControl.Domain.AggregatesModel.FraudAggregate;
using RiskControl.Domain.AggregatesModel.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RiskControl.Infrastructure.EntityConfigurations;

public class FraudCheckEntityTypeConfiguration : IEntityTypeConfiguration<FraudCheck>
{
    public void Configure(EntityTypeBuilder<FraudCheck> builder)
    {
        builder.ToTable("FraudChecks");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .UseGuidVersion7ValueGenerator()
            .HasComment("反欺诈检测ID");

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasComment("订单ID");

        builder.Property(x => x.Channel)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("渠道");

        builder.Property(x => x.Amount)
            .IsRequired()
            .HasComment("金额");

        builder.Property(x => x.IpAddress)
            .IsRequired()
            .HasMaxLength(45)
            .HasComment("IP地址");

        builder.Property(x => x.RiskScore)
            .IsRequired()
            .HasComment("风险分");

        builder.Property(x => x.Result)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("结果");

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("状态");

        builder.Property(x => x.Reasons)
            .IsRequired()
            .HasMaxLength(500)
            .HasComment("原因");

        builder.Property(x => x.CheckedAt)
            .IsRequired()
            .HasComment("检测时间");

        builder.HasIndex(x => new { x.OrderId, x.CheckedAt });
    }
}
