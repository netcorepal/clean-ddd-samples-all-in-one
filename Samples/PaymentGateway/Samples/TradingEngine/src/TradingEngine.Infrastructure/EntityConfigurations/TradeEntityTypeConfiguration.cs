using TradingEngine.Domain.AggregatesModel.TradeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TradingEngine.Infrastructure.EntityConfigurations;

public class TradeEntityTypeConfiguration : IEntityTypeConfiguration<Trade>
{
    public void Configure(EntityTypeBuilder<Trade> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Id)
            .UseGuidVersion7ValueGenerator()
            .HasComment("交易ID");

        builder.Property(t => t.Symbol)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("交易标的");

        builder.Property(t => t.TradeType)
            .IsRequired()
            .HasConversion<int>()
            .HasComment("交易类型：1-买入，2-卖出");

        builder.Property(t => t.Quantity)
            .IsRequired()
            .HasPrecision(18, 8)
            .HasComment("交易数量");

        builder.Property(t => t.Price)
            .IsRequired()
            .HasPrecision(18, 8)
            .HasComment("交易价格");

        builder.Property(t => t.ExecutedQuantity)
            .IsRequired()
            .HasPrecision(18, 8)
            .HasComment("已执行数量");

        builder.Property(t => t.RemainingQuantity)
            .IsRequired()
            .HasPrecision(18, 8)
            .HasComment("剩余数量");

        builder.Property(t => t.TotalValue)
            .IsRequired()
            .HasPrecision(18, 8)
            .HasComment("交易总值");

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasComment("交易状态：1-待处理，2-已执行，3-失败，4-已取消，5-部分成交");

        builder.Property(t => t.UserId)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("用户ID");

        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasComment("创建时间");

        builder.Property(t => t.ExecutedAt)
            .HasComment("执行时间");

        builder.Property(t => t.FailureReason)
            .HasMaxLength(500)
            .HasComment("失败原因");

        // 索引
        builder.HasIndex(t => t.UserId)
            .HasDatabaseName("IX_Trades_UserId");

        builder.HasIndex(t => t.Symbol)
            .HasDatabaseName("IX_Trades_Symbol");

        builder.HasIndex(t => t.Status)
            .HasDatabaseName("IX_Trades_Status");

        builder.HasIndex(t => t.CreatedAt)
            .HasDatabaseName("IX_Trades_CreatedAt");

        builder.ToTable("Trades");
    }
}
