using Catalog_Service.src._01_Domain.Core.Entities;
using Catalog_Service.src._01_Domain.Core.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog_Service.src._02_Infrastructure.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasMaxLength(1000);

            builder.Property(c => c.CreatedByUserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(c => c.DisplayOrder)
                .IsRequired();

            builder.Property(c => c.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(c => c.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .IsRequired(false);

            builder.Property(c => c.ImageUrl)
                .HasMaxLength(500);

            builder.Property(c => c.MetaTitle)
                .HasMaxLength(200);

            builder.Property(c => c.MetaDescription)
                .HasMaxLength(500);

            builder.Property(c => c.Slug)
                .HasConversion(
                    slug => slug.Value,
                    value => Slug.FromString(value))
                .HasMaxLength(200);

            // Self-referencing relationship
            builder.HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => c.ParentCategoryId);
            builder.HasIndex(c => c.DisplayOrder);
            builder.HasIndex(c => c.Slug);

            builder.HasCheckConstraint("CK_Category_NoCircularReference",
                "Id <> ParentCategoryId OR ParentCategoryId IS NULL");
        }
    }
}