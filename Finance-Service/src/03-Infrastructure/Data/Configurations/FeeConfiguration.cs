using Finance_Service.src._01_Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance_Service.src._03_Infrastructure.Data.Configurations
{
    public class FeeConfiguration : IEntityTypeConfiguration<Fee>
    {
        public void Configure(EntityTypeBuilder<Fee> builder)
        {
            builder.ToTable("Fees");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.OrderId).IsRequired();

            builder.OwnsOne(f => f.Amount, money =>
            {
                money.Property(m => m.Amount).HasColumnName("Amount").HasPrecision(18, 0).IsRequired();
                money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
            });

            builder.Property(f => f.Type).IsRequired();
            builder.Property(f => f.Description).HasMaxLength(500);
            builder.Property(f => f.SellerId);
            builder.Property(f => f.IsPaid).IsRequired();
            builder.Property(f => f.PaidAt);

            builder.OwnsOne(f => f.AuditInfo, audit =>
            {
                audit.Property(a => a.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(100);
                audit.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
                audit.Property(a => a.LastModifiedBy).HasColumnName("LastModifiedBy").HasMaxLength(100);
                audit.Property(a => a.LastModifiedAt).HasColumnName("LastModifiedAt");
            });

            builder.Property(f => f.CreatedAt).IsRequired();
            builder.Property(f => f.UpdatedAt);
            builder.Property(f => f.IsDeleted).HasDefaultValue(false);
        }
    }
}
