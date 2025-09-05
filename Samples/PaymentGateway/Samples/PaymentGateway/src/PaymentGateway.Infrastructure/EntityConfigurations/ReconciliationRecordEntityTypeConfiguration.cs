using PaymentGateway.Domain.AggregatesModel.ReconciliationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaymentGateway.Infrastructure.EntityConfigurations;

internal class ReconciliationRecordEntityTypeConfiguration : IEntityTypeConfiguration<ReconciliationRecord>
{
    public void Configure(EntityTypeBuilder<ReconciliationRecord> builder)
    {
        builder.ToTable("reconciliation_record");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseSnowFlakeValueGenerator();

        builder.Property(x => x.Provider)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(x => x.ProviderTransactionId)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(x => x.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        builder.Property(x => x.Currency)
            .HasMaxLength(10)
            .IsRequired();
        builder.Property(x => x.OccurTime)
            .IsRequired();
        builder.Property(x => x.PaymentId);
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.Note).HasMaxLength(200);

        builder.HasIndex(x => new { x.Provider, x.ProviderTransactionId }).IsUnique();
        builder.HasIndex(x => x.PaymentId);
        builder.HasIndex(x => x.Status);
    }
}
