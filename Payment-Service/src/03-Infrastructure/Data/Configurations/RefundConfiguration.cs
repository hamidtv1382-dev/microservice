using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment_Service.src._01_Domain.Core.Entities;

namespace Payment_Service.src._03_Infrastructure.Data.Configurations
{
    public class RefundConfiguration : IEntityTypeConfiguration<Refund>
    {
        public void Configure(EntityTypeBuilder<Refund> builder)
        {
            builder.ToTable("Refunds");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.PaymentId).IsRequired();

            builder.OwnsOne(r => r.Amount, money =>
            {
                money.Property(m => m.Amount).HasColumnName("Amount").HasPrecision(18, 0).IsRequired();
                money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
            });

            builder.Property(r => r.Status).IsRequired();
            builder.Property(r => r.Reason).HasMaxLength(500).IsRequired();
            builder.Property(r => r.ExternalRefundId).HasMaxLength(100);
            builder.Property(r => r.RefundedAt);

            builder.OwnsOne(r => r.AuditInfo, audit =>
            {
                audit.Property(a => a.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(100);
                audit.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
                audit.Property(a => a.LastModifiedBy).HasColumnName("LastModifiedBy").HasMaxLength(100);
                audit.Property(a => a.LastModifiedAt).HasColumnName("LastModifiedAt");
            });

            builder.Property(r => r.CreatedAt).IsRequired();
            builder.Property(r => r.UpdatedAt);
            builder.Property(r => r.IsDeleted).HasDefaultValue(false);

            // Ignore lines removed
        }
    }
}
