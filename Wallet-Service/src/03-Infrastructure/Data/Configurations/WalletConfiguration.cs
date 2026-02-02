using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet_Service.src._01_Domain.Core.Entities;

namespace Wallet_Service.src._03_Infrastructure.Data.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallets");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.OwnerId).IsRequired();

            builder.OwnsOne(w => w.Balance, money =>
            {
                money.Property(m => m.Amount).HasColumnName("BalanceAmount").HasPrecision(18, 0).IsRequired();
                money.Property(m => m.Currency).HasColumnName("BalanceCurrency").HasMaxLength(3).IsRequired();
            });

            builder.Property(w => w.IsActive).IsRequired();

            builder.OwnsOne(w => w.AuditInfo, audit =>
            {
                audit.Property(a => a.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(100);
                audit.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
                audit.Property(a => a.LastModifiedBy).HasColumnName("LastModifiedBy").HasMaxLength(100);
                audit.Property(a => a.LastModifiedAt).HasColumnName("LastModifiedAt");
            });

            builder.Property(w => w.CreatedAt).IsRequired();
            builder.Property(w => w.UpdatedAt);
            builder.Property(w => w.IsDeleted).HasDefaultValue(false);

            // Navigation to transactions is ignored as it is a read-only collection in domain
            builder.Ignore(w => w.Transactions);
        }
    }
}
