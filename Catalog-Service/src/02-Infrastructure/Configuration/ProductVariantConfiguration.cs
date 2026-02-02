using Catalog_Service.src._01_Domain.Core.Entities;
using Catalog_Service.src._01_Domain.Core.Enums;
using Catalog_Service.src._02_Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog_Service.src._02_Infrastructure.Configuration
{
    public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.ToTable("ProductVariants");

            builder.HasKey(pv => pv.Id);

            builder.Property(pv => pv.ProductId)
                .IsRequired();

            builder.Property(pv => pv.Sku)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(pv => pv.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(pv => pv.Price)
                .HasConversion<MoneyConverter>();

            builder.Property(pv => pv.OriginalPrice)
                .HasConversion<MoneyConverter>();

            builder.Property(pv => pv.StockQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(pv => pv.StockStatus)
                .IsRequired()
                .HasDefaultValue(StockStatus.OutOfStock);

            builder.Property(pv => pv.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(pv => pv.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(pv => pv.ImageUrl)
                .HasMaxLength(500);

            builder.HasOne(pv => pv.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(pv => pv.Attributes)
                .WithOne(a => a.ProductVariant)
                .HasForeignKey(a => a.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(pv => pv.ProductId);
            builder.HasIndex(pv => pv.Sku).IsUnique();
            builder.HasIndex(pv => pv.StockStatus);
            builder.HasIndex(pv => pv.IsActive);
        }
    }
}