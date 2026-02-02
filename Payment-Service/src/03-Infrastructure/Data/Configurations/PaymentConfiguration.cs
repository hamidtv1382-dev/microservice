using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment_Service.src._01_Domain.Core.Entities;

namespace Payment_Service.src._03_Infrastructure.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.OrderId).IsRequired();

            builder.OwnsOne(p => p.Amount, money =>
            {
                money.Property(m => m.Amount).HasColumnName("Amount").HasPrecision(18, 0).IsRequired();
                money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
            });

            builder.Property(p => p.Method).IsRequired();
            builder.Property(p => p.Status).IsRequired();

            builder.OwnsOne(p => p.TransactionNumber, tn =>
            {
                tn.Property(t => t.Value).HasColumnName("TransactionNumber").HasMaxLength(50).IsRequired();
            });

            builder.Property(p => p.ExternalTransactionId).HasMaxLength(100);
            builder.Property(p => p.FailureReason).HasMaxLength(500);
            builder.Property(p => p.PaidAt);

            builder.OwnsOne(p => p.AuditInfo, audit =>
            {
                audit.Property(a => a.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(100);
                audit.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
                audit.Property(a => a.LastModifiedBy).HasColumnName("LastModifiedBy").HasMaxLength(100);
                audit.Property(a => a.LastModifiedAt).HasColumnName("LastModifiedAt");
            });

            builder.Property(p => p.CreatedAt).IsRequired();
            builder.Property(p => p.UpdatedAt);
            builder.Property(p => p.IsDeleted).HasDefaultValue(false);

            // Ignore lines removed
        }
    }
}
