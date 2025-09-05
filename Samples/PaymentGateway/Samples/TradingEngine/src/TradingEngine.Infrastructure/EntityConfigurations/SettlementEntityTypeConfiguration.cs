using TradingEngine.Domain.AggregatesModel.SettlementAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TradingEngine.Infrastructure.EntityConfigurations;

public class SettlementEntityTypeConfiguration : IEntityTypeConfiguration<Settlement>
{
    public void Configure(EntityTypeBuilder<Settlement> builder)
    {
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Id)
            .UseGuidVersion7ValueGenerator()
            .HasComment("结算ID");

        builder.Property(s => s.UserId)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("用户ID");

        builder.Property(s => s.SettlementType)
            .IsRequired()
            .HasConversion<int>()
            .HasComment("结算类型：1-交易结算，2-分红结算，3-费用结算，4-保证金调整");

        builder.Property(s => s.TotalAmount)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasComment("结算总金额");

        builder.Property(s => s.SettlementDate)
            .IsRequired()
            .HasComment("结算日期");

        builder.Property(s => s.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasComment("结算状态：1-待处理，2-处理中，3-已完成，4-失败，5-已取消");

        builder.Property(s => s.CreatedAt)
            .IsRequired()
            .HasComment("创建时间");

        builder.Property(s => s.ProcessedAt)
            .HasComment("处理时间");

        builder.Property(s => s.CompletedAt)
            .HasComment("完成时间");

        builder.Property(s => s.FailureReason)
            .HasMaxLength(500)
            .HasComment("失败原因");

        // 配置子实体
        builder.OwnsMany(s => s.Items, si =>
        {
            si.WithOwner().HasForeignKey("SettlementId");
            si.HasKey(nameof(SettlementItem.Id));
            
            si.Property(i => i.Id)
                .UseGuidVersion7ValueGenerator()
                .HasComment("结算项目ID");

            si.Property(i => i.ReferenceId)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("关联引用ID");

            si.Property(i => i.Symbol)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("交易标的");

            si.Property(i => i.Quantity)
                .IsRequired()
                .HasPrecision(18, 8)
                .HasComment("数量");

            si.Property(i => i.Price)
                .IsRequired()
                .HasPrecision(18, 8)
                .HasComment("价格");

            si.Property(i => i.Amount)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasComment("结算金额");

            si.Property(i => i.Description)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("描述");

            si.Property(i => i.CreatedAt)
                .IsRequired()
                .HasComment("创建时间");

            si.ToTable("SettlementItems");
        });

        // 索引
        builder.HasIndex(s => s.UserId)
            .HasDatabaseName("IX_Settlements_UserId");

        builder.HasIndex(s => s.Status)
            .HasDatabaseName("IX_Settlements_Status");

        builder.HasIndex(s => s.SettlementDate)
            .HasDatabaseName("IX_Settlements_SettlementDate");

        builder.HasIndex(s => s.SettlementType)
            .HasDatabaseName("IX_Settlements_SettlementType");

        builder.ToTable("Settlements");
    }
}
