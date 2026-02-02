using Catalog_Service.src._01_Domain.Core.Entities;
using Catalog_Service.src._01_Domain.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog_Service.src._02_Infrastructure.Configuration
{
    public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
    {
        public void Configure(EntityTypeBuilder<ProductReview> builder)
        {
            builder.ToTable("ProductReviews");

            builder.HasKey(pr => pr.Id);

            builder.Property(pr => pr.ProductId)
                .IsRequired();

            builder.Property(pr => pr.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(pr => pr.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(pr => pr.Comment)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(pr => pr.Rating)
                .IsRequired();

            builder.Property(pr => pr.Status)
                .IsRequired()
                .HasDefaultValue(ReviewStatus.Pending);

            builder.Property(pr => pr.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(pr => pr.IsVerifiedPurchase)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(pr => pr.HelpfulVotes)
                .IsRequired()
                .HasDefaultValue(0);

            // Relationship with Product
            builder.HasOne(pr => pr.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(pr => pr.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // این می‌تواند Cascade باشد

            // Relationships for Replies and Images -> تغییر به Restrict
            builder.HasMany(pr => pr.Replies)
                .WithOne()
                .HasForeignKey(r => r.ProductReviewId)
                .OnDelete(DeleteBehavior.Restrict);

            // Images relationship is handled in ImageResourceConfiguration, 
            // but ensuring consistency here if navigation is used.

            builder.HasIndex(pr => pr.ProductId);
            builder.HasIndex(pr => pr.UserId);
            builder.HasIndex(pr => pr.Rating);
            builder.HasIndex(pr => pr.Status);

            builder.HasIndex(pr => new { pr.ProductId, pr.UserId })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.HasCheckConstraint("CK_ProductReview_ValidRating", "Rating >= 1 AND Rating <= 5");
        }
    }
}