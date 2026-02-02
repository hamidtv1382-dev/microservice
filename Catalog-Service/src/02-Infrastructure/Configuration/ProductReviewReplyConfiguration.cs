using Catalog_Service.src._01_Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog_Service.src._02_Infrastructure.Configuration
{
    public class ProductReviewReplyConfiguration : IEntityTypeConfiguration<ProductReviewReply>
    {
        public void Configure(EntityTypeBuilder<ProductReviewReply> builder)
        {
            builder.ToTable("ProductReviewReplies");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.ProductReviewId)
                .IsRequired();

            builder.Property(r => r.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(r => r.Comment)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.Property(r => r.UpdatedAt)
                .IsRequired(false);

            // Relationship -> تغییر به Restrict برای جلوگیری از خطای Multiple Cascade Paths
            builder.HasOne(r => r.ProductReview)
                .WithMany(pr => pr.Replies)
                .HasForeignKey(r => r.ProductReviewId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(r => r.ProductReviewId);
        }
    }
}