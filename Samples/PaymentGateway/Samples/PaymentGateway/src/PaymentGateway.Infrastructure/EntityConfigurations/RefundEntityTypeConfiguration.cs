using PaymentGateway.Domain.AggregatesModel.RefundAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaymentGateway.Infrastructure.EntityConfigurations;

internal class RefundEntityTypeConfiguration : IEntityTypeConfiguration<Refund>
{
    public void Configure(EntityTypeBuilder<Refund> builder)
    {
        builder.ToTable("refund");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseSnowFlakeValueGenerator();

        builder.Property(x => x.PaymentId)
            .IsRequired();
        builder.Property(x => x.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        builder.Property(x => x.Reason)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(x => x.Status)
            .IsRequired();
        builder.Property(x => x.ProviderRefundId)
            .HasMaxLength(100);
        builder.Property(x => x.CreatedTime).IsRequired();
        builder.Property(x => x.SucceededTime);
        builder.Property(x => x.FailedTime);

        builder.HasIndex(x => x.PaymentId);
        builder.HasIndex(x => x.ProviderRefundId);
    }
}
