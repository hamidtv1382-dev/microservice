using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Review_Rating_Service.src._01_Domain.Core.Aggregates.Review;

namespace Review_Rating_Service.src._03_Infrastructure.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.ProductId).IsRequired();
            builder.Property(r => r.UserId).IsRequired();

            // ReviewerName ValueObject mapping
            builder.OwnsOne(r => r.ReviewerName, name =>
            {
                name.Property(n => n.Value).HasColumnName("ReviewerName").HasMaxLength(100).IsRequired();
            });

            // ReviewDate ValueObject mapping
            builder.OwnsOne(r => r.ReviewDate, date =>
            {
                date.Property(d => d.Value).HasColumnName("ReviewDate").IsRequired();
            });

            // Text Property (Stored directly in Review table)
            builder.Property(r => r.Text).HasMaxLength(2000).IsRequired();

            builder.Property(r => r.Status).HasConversion<int>().IsRequired();

            // Collections
            builder.Metadata.FindNavigation(nameof(Review.Ratings))!.SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Review.Comments))!.SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Review.Attachments))!.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(r => r.CreatedAt).IsRequired();
            builder.Property(r => r.UpdatedAt);
            builder.Property(r => r.IsDeleted).HasDefaultValue(false);
        }
    }
}
