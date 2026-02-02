using Catalog_Service.src._01_Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog_Service.src._02_Infrastructure.Configuration
{
    public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductTag> builder)
        {
            builder.ToTable("ProductTags");

            builder.HasKey(pt => pt.Id);

            builder.Property(pt => pt.ProductId)
                .IsRequired();

            builder.Property(pt => pt.TagText)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(pt => pt.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne(pt => pt.Product)
                .WithMany(p => p.Tags)
                .HasForeignKey(pt => pt.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(pt => pt.ProductId);
            builder.HasIndex(pt => pt.TagText);

            // Ensure unique tag per product (case-insensitive logic usually handled in app layer, but unique index in DB helps)
            builder.HasIndex(pt => new { pt.ProductId, pt.TagText })
                .IsUnique();
        }
    }
}