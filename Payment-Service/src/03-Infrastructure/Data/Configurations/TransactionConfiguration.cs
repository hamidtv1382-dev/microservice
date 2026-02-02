using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment_Service.src._01_Domain.Core.Entities;

namespace PaymentService.src._03_Infrastructure.Data.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.PaymentId).IsRequired();

            builder.OwnsOne(t => t.Amount, money =>
            {
                money.Property(m => m.Amount).HasColumnName("Amount").HasPrecision(18, 0).IsRequired();
                money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
            });

            builder.Property(t => t.Type).IsRequired();

            builder.OwnsOne(t => t.TransactionNumber, tn =>
            {
                tn.Property(t => t.Value).HasColumnName("TransactionNumber").HasMaxLength(50).IsRequired();
            });

            builder.Property(t => t.GatewayResponse).HasMaxLength(1000);
            builder.Property(t => t.ProcessedAt).IsRequired();

            builder.OwnsOne(t => t.AuditInfo, audit =>
            {
                audit.Property(a => a.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(100);
                audit.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
                audit.Property(a => a.LastModifiedBy).HasColumnName("LastModifiedBy").HasMaxLength(100);
                audit.Property(a => a.LastModifiedAt).HasColumnName("LastModifiedAt");
            });

            builder.Property(t => t.CreatedAt).IsRequired();
            builder.Property(t => t.IsDeleted).HasDefaultValue(false);
        }
    }
}