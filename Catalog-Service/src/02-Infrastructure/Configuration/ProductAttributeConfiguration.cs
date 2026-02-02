using Catalog_Service.src._01_Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog_Service.src._02_Infrastructure.Configuration
{
    public class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            builder.ToTable("ProductAttributes");

            builder.HasKey(pa => pa.Id);

            builder.Property(pa => pa.ProductId)
                .IsRequired();

            builder.Property(pa => pa.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(pa => pa.Value)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(pa => pa.IsVariantSpecific)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(pa => pa.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // -------------------------------------------------------
            // تغییرات کلیدی برای حل خطای Cascade Path:
            // هر دو رابطه را به Restrict تغییر دادیم.
            // -------------------------------------------------------

            builder.HasOne(pa => pa.Product)
                .WithMany(p => p.Attributes)
                .HasForeignKey(pa => pa.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // قبلاً Cascade بود

            builder.HasOne(pa => pa.ProductVariant)
                .WithMany(pv => pv.Attributes)
                .HasForeignKey(pa => pa.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict); // قبلاً Cascade بود

            builder.HasIndex(pa => pa.ProductId);
            builder.HasIndex(pa => pa.ProductVariantId);
            builder.HasIndex(pa => pa.Name);

            builder.HasCheckConstraint("CK_ProductAttribute_SingleEntityReference",
                "(ProductVariantId IS NULL AND IsVariantSpecific = 0) OR " +
                "(ProductVariantId IS NOT NULL AND IsVariantSpecific = 1)");
        }
    }
}