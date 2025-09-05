using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaymentGateway.Infrastructure.EntityConfigurations;

internal class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payment");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseSnowFlakeValueGenerator();

        builder.Property(x => x.OrderId)
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        builder.Property(x => x.Currency)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.Channel)
            .IsRequired();
        builder.Property(x => x.Status)
            .IsRequired();
        builder.Property(x => x.ProviderTransactionId)
            .HasMaxLength(100);
        builder.Property(x => x.CreatedTime)
            .IsRequired();
        builder.Property(x => x.SucceededTime);
        builder.Property(x => x.FailedTime);

        builder.HasIndex(x => x.OrderId);
        builder.HasIndex(x => x.ProviderTransactionId);
    }
}
