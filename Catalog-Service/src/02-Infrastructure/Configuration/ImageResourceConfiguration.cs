using Catalog_Service.src._01_Domain.Core.Entities;
using Catalog_Service.src._01_Domain.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog_Service.src._02_Infrastructure.Configuration
{
    public class ImageResourceConfiguration : IEntityTypeConfiguration<ImageResource>
    {
        public void Configure(EntityTypeBuilder<ImageResource> builder)
        {
            builder.ToTable("ImageResources");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.OriginalFileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(i => i.FileExtension)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(i => i.StoragePath)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(i => i.PublicUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(i => i.FileSize)
                .IsRequired();

            builder.Property(i => i.Width)
                .IsRequired();

            builder.Property(i => i.Height)
                .IsRequired();

            builder.Property(i => i.ImageType)
                .IsRequired()
                .HasDefaultValue(ImageType.Product);

            builder.Property(i => i.CreatedByUserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(i => i.IsPrimary)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(i => i.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(i => i.AltText)
                .HasMaxLength(200);

            // Shadow Properties
            builder.Property<int?>("ProductId");
            builder.Property<int?>("CategoryId");
            builder.Property<int?>("BrandId");
            builder.Property<int?>("ProductVariantId");
            builder.Property<int?>("ProductReviewId");

            // Relationships (همه را Restrict کنید تا از Cascade Path جلوگیری شود)

            // 1. Product -> Images (تغییر یافته به Restrict)
            builder.HasOne<Product>()
                .WithMany(p => p.Images)
                .HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Category -> Image
            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey("CategoryId")
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Brand -> Image
            builder.HasOne<Brand>()
                .WithMany()
                .HasForeignKey("BrandId")
                .OnDelete(DeleteBehavior.Restrict);

            // 4. ProductVariant -> Image
            builder.HasOne<ProductVariant>()
                .WithMany()
                .HasForeignKey("ProductVariantId")
                .OnDelete(DeleteBehavior.Restrict);

            // 5. ProductReview -> Images (اینجا را هم Restrict کنید تا با Review Replies تداخل نداشته باشد)
            builder.HasOne<ProductReview>()
                .WithMany(pr => pr.Images)
                .HasForeignKey("ProductReviewId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(i => i.ImageType);
            builder.HasIndex(i => i.IsPrimary);
            builder.HasIndex("ProductId");
            builder.HasIndex("ProductReviewId");

            builder.HasCheckConstraint("CK_ImageResource_SingleEntityReference",
                "(ProductId IS NOT NULL AND CategoryId IS NULL AND BrandId IS NULL AND ProductVariantId IS NULL AND ProductReviewId IS NULL) OR " +
                "(ProductId IS NULL AND CategoryId IS NOT NULL AND BrandId IS NULL AND ProductVariantId IS NULL AND ProductReviewId IS NULL) OR " +
                "(ProductId IS NULL AND CategoryId IS NULL AND BrandId IS NOT NULL AND ProductVariantId IS NULL AND ProductReviewId IS NULL) OR " +
                "(ProductId IS NULL AND CategoryId IS NULL AND BrandId IS NULL AND ProductVariantId IS NOT NULL AND ProductReviewId IS NULL) OR " +
                "(ProductId IS NULL AND CategoryId IS NULL AND BrandId IS NULL AND ProductVariantId IS NULL AND ProductReviewId IS NOT NULL)");
        }
    }
}