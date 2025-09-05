using TradingEngine.Domain.AggregatesModel.RiskControlAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TradingEngine.Infrastructure.EntityConfigurations;

public class RiskControlEntityTypeConfiguration : IEntityTypeConfiguration<RiskControl>
{
    public void Configure(EntityTypeBuilder<RiskControl> builder)
    {
        builder.HasKey(rc => rc.Id);
        
        builder.Property(rc => rc.Id)
            .UseGuidVersion7ValueGenerator()
            .HasComment("风险控制ID");

        builder.Property(rc => rc.UserId)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("用户ID");

        builder.Property(rc => rc.TotalPositionLimit)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasComment("总持仓限制");

        builder.Property(rc => rc.DailyLossLimit)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasComment("日损失限制");

        builder.Property(rc => rc.CurrentPosition)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasComment("当前持仓");

        builder.Property(rc => rc.DailyLoss)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasComment("当日损失");

        builder.Property(rc => rc.IsActive)
            .IsRequired()
            .HasComment("是否激活");

        builder.Property(rc => rc.CreatedAt)
            .IsRequired()
            .HasComment("创建时间");

        builder.Property(rc => rc.LastAssessmentAt)
            .HasComment("最后评估时间");

        // 配置子实体
        builder.OwnsMany(rc => rc.RiskAssessments, ra =>
        {
            ra.WithOwner().HasForeignKey("RiskControlId");
            ra.HasKey(nameof(RiskAssessment.Id));
            
            ra.Property(r => r.Id)
                .UseGuidVersion7ValueGenerator()
                .HasComment("风险评估ID");

            ra.Property(r => r.Symbol)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("交易标的");

            ra.Property(r => r.Quantity)
                .IsRequired()
                .HasPrecision(18, 8)
                .HasComment("交易数量");

            ra.Property(r => r.Price)
                .IsRequired()
                .HasPrecision(18, 8)
                .HasComment("交易价格");

            ra.Property(r => r.TradeType)
                .IsRequired()
                .HasConversion<int>()
                .HasComment("交易类型：1-买入，2-卖出");

            ra.Property(r => r.RiskLevel)
                .IsRequired()
                .HasConversion<int>()
                .HasComment("风险等级：1-低，2-中，3-高，4-严重");

            ra.Property(r => r.RiskTypes)
                .IsRequired()
                .HasConversion(
                    v => string.Join(',', v.Select(rt => (int)rt)),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(s => (RiskType)int.Parse(s)).ToList())
                .HasComment("风险类型列表");

            ra.Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(1000)
                .HasComment("风险描述");

            ra.Property(r => r.AssessedAt)
                .IsRequired()
                .HasComment("评估时间");

            ra.ToTable("RiskAssessments");
        });

        // 索引
        builder.HasIndex(rc => rc.UserId)
            .IsUnique()
            .HasDatabaseName("IX_RiskControls_UserId");

        builder.HasIndex(rc => rc.IsActive)
            .HasDatabaseName("IX_RiskControls_IsActive");

        builder.ToTable("RiskControls");
    }
}
