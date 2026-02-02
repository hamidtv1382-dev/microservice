using Finance_Service.src._01_Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance_Service.src._03_Infrastructure.Data.Configurations
{
    public class CommissionConfiguration : IEntityTypeConfiguration<Commission>
    {
        public void Configure(EntityTypeBuilder<Commission> builder)
        {
            builder.ToTable("Commissions");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.OrderId).IsRequired();
            builder.Property(c => c.SellerId).IsRequired();

            builder.OwnsOne(c => c.Amount, money =>
            {
                money.Property(m => m.Amount).HasColumnName("Amount").HasPrecision(18, 0).IsRequired();
                money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
            });

            builder.Property(c => c.Type).IsRequired();

            // FIX: Remove Ignore for Currency so the constructor 'Money(decimal, string)' can be satisfied
            builder.OwnsOne(c => c.RatePercentage, rate =>
            {
                rate.Property(r => r.Amount).HasColumnName("RatePercentage").HasPrecision(5, 2).IsRequired();
                rate.Property(r => r.Currency).HasColumnName("RateCurrency").HasMaxLength(10).IsRequired(); // Changed column name to avoid collision
            });

            builder.Property(c => c.IsSettled).IsRequired();
            builder.Property(c => c.SettledAt);

            builder.OwnsOne(c => c.AuditInfo, audit =>
            {
                audit.Property(a => a.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(100);
                audit.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
                audit.Property(a => a.LastModifiedBy).HasColumnName("LastModifiedBy").HasMaxLength(100);
                audit.Property(a => a.LastModifiedAt).HasColumnName("LastModifiedAt");
            });

            builder.Property(c => c.CreatedAt).IsRequired();
            builder.Property(c => c.UpdatedAt);
            builder.Property(c => c.IsDeleted).HasDefaultValue(false);
        }
    }
}
