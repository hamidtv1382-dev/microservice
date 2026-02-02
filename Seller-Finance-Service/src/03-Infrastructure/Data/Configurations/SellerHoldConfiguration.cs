using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seller_Finance_Service.src._01_Domain.Core.Entities;

namespace Seller_Finance_Service.src._03_Infrastructure.Data.Configurations
{

    public class SellerHoldConfiguration : IEntityTypeConfiguration<SellerHold>
    {
        public void Configure(EntityTypeBuilder<SellerHold> builder)
        {
            builder.ToTable("SellerHolds");
            builder.HasKey(h => h.Id);
            builder.Property(h => h.SellerAccountId).IsRequired();

            // Money Mapping
            builder.Ignore(h => h.Amount);
            builder.Property<decimal>("Amount_Value").HasColumnName("Amount").HasPrecision(18, 0).IsRequired();

            builder.Property(h => h.Reason).IsRequired();
            builder.Property(h => h.Description).HasMaxLength(500);
            builder.Property(h => h.IsReleased).IsRequired();
            builder.Property(h => h.ReleasedAt);

            builder.OwnsOne(h => h.AuditInfo, audit =>
            {
                audit.Property(a => a.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(100);
                audit.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
                audit.Property(a => a.LastModifiedBy).HasColumnName("LastModifiedBy").HasMaxLength(100);
                audit.Property(a => a.LastModifiedAt).HasColumnName("LastModifiedAt");
            });

            builder.Property(h => h.CreatedAt).IsRequired();
            builder.Property(h => h.UpdatedAt);
            builder.Property(h => h.IsDeleted).HasDefaultValue(false);
        }
    }
    
}
