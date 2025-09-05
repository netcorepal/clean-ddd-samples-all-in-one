using RiskControl.Domain.AggregatesModel.CreditAggregate;
using RiskControl.Domain.AggregatesModel.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RiskControl.Infrastructure.EntityConfigurations;

public class CreditAssessmentEntityTypeConfiguration : IEntityTypeConfiguration<CreditAssessment>
{
    public void Configure(EntityTypeBuilder<CreditAssessment> builder)
    {
        builder.ToTable("CreditAssessments");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .UseGuidVersion7ValueGenerator()
            .HasComment("信用评估ID");

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasComment("订单ID");

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasMaxLength(64)
            .HasComment("客户ID");

        builder.Property(x => x.Exposure)
            .IsRequired()
            .HasComment("额度暴露");

        builder.Property(x => x.Score)
            .IsRequired()
            .HasComment("信用分");

        builder.Property(x => x.Grade)
            .IsRequired()
            .HasMaxLength(10)
            .HasComment("评级");

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("状态");

        builder.Property(x => x.AssessedAt)
            .IsRequired()
            .HasComment("评估时间");

        builder.HasIndex(x => new { x.CustomerId, x.AssessedAt });
    }
}
