using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seller_Finance_Service.src._01_Domain.Core.Entities;

namespace Seller_Finance_Service.src._03_Infrastructure.Data.Configurations
{
    public class SellerPayoutConfiguration : IEntityTypeConfiguration<SellerPayout>
    {
        public void Configure(EntityTypeBuilder<SellerPayout> builder)
        {
            builder.ToTable("SellerPayouts");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.SellerAccountId).IsRequired();

            // Money Mapping
            builder.Ignore(p => p.Amount);
            builder.Property<decimal>("Amount_Value").HasColumnName("Amount").HasPrecision(18, 0).IsRequired();

            builder.Property(p => p.Status).IsRequired();
            builder.Property(p => p.GatewayReferenceId).HasMaxLength(100);
            builder.Property(p => p.RequestedAt).IsRequired();
            builder.Property(p => p.ProcessedAt);
            builder.Property(p => p.FailureReason).HasMaxLength(500);

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
        }
    }
}
