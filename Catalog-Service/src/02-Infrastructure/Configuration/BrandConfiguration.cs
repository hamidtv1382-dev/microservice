using Catalog_Service.src._01_Domain.Core.Entities;
using Catalog_Service.src._01_Domain.Core.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog_Service.src._02_Infrastructure.Configuration
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brands");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.Description)
                .HasMaxLength(1000);

            builder.Property(b => b.CreatedByUserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(b => b.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(b => b.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.Property(b => b.UpdatedAt)
                .IsRequired(false);

            builder.Property(b => b.LogoUrl)
                .HasMaxLength(500);

            builder.Property(b => b.WebsiteUrl)
                .HasMaxLength(500);

            builder.Property(b => b.MetaTitle)
                .HasMaxLength(200);

            builder.Property(b => b.MetaDescription)
                .HasMaxLength(500);

            builder.Property(b => b.Slug)
                .HasConversion(
                 slug => slug.Value,
                 value => Slug.FromString(value))
                .HasMaxLength(200);

            builder.HasIndex(b => b.IsActive);
            builder.HasIndex(b => b.Slug);
        }
    }
}