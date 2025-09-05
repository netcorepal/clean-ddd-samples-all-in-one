using FinancialServices.Backend.Domain.AggregatesModel.AccountAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialServices.Backend.Infrastructure.EntityConfigurations;

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .UseGuidVersion7ValueGenerator()
            .HasComment("账户标识");

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("邮箱地址");

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("全名");

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("手机号");

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasComment("账户状态：0-待审核，1-活跃，2-暂停，3-关闭");

        builder.Property(x => x.KycStatus)
            .IsRequired()
            .HasConversion<int>()
            .HasComment("KYC状态：0-未开始，1-进行中，2-已通过，3-已拒绝");

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasComment("创建时间");

        builder.Property(x => x.KycSubmittedAt)
            .HasComment("KYC提交时间");

        builder.Property(x => x.KycApprovedAt)
            .HasComment("KYC通过时间");

        builder.Property(x => x.KycRejectionReason)
            .HasMaxLength(500)
            .HasComment("KYC拒绝原因");

        builder.Property(x => x.IdentityDocumentType)
            .HasMaxLength(50)
            .HasComment("身份证件类型");

        builder.Property(x => x.IdentityDocumentNumber)
            .HasMaxLength(100)
            .HasComment("身份证件号码");

        // 索引配置
        builder.HasIndex(x => x.Email)
            .IsUnique()
            .HasDatabaseName("IX_Accounts_Email");

        builder.HasIndex(x => x.PhoneNumber)
            .IsUnique()
            .HasDatabaseName("IX_Accounts_PhoneNumber");

        builder.HasIndex(x => x.KycStatus)
            .HasDatabaseName("IX_Accounts_KycStatus");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_Accounts_Status");
    }
}
