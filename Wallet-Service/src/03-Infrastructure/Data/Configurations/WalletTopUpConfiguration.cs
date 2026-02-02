using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet_Service.src._01_Domain.Core.Entities;

namespace Wallet_Service.src._03_Infrastructure.Data.Configurations
{
    public class WalletTopUpConfiguration : IEntityTypeConfiguration<WalletTopUp>
    {
        public void Configure(EntityTypeBuilder<WalletTopUp> builder)
        {
            builder.ToTable("WalletTopUps");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.WalletId).IsRequired();

            builder.OwnsOne(t => t.Amount, money =>
            {
                money.Property(m => m.Amount).HasColumnName("Amount").HasPrecision(18, 0).IsRequired();
                money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
            });

            builder.Property(t => t.GatewayTransactionId).HasMaxLength(100);
            builder.Property(t => t.Status).IsRequired();
            builder.Property(t => t.CompletedAt);

            builder.OwnsOne(t => t.AuditInfo, audit =>
            {
                audit.Property(a => a.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(100);
                audit.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
                audit.Property(a => a.LastModifiedBy).HasColumnName("LastModifiedBy").HasMaxLength(100);
                audit.Property(a => a.LastModifiedAt).HasColumnName("LastModifiedAt");
            });

            builder.Property(t => t.CreatedAt).IsRequired();
            builder.Property(t => t.UpdatedAt);
            builder.Property(t => t.IsDeleted).HasDefaultValue(false);
        }
    }
}
