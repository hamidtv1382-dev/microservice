using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Review_Rating_Service.src._01_Domain.Core.Aggregates.Review;

namespace Review_Rating_Service.src._03_Infrastructure.Data.Configurations
{
    public class ReviewCommentConfiguration : IEntityTypeConfiguration<ReviewComment>
    {
        public void Configure(EntityTypeBuilder<ReviewComment> builder)
        {
            builder.ToTable("ReviewComments");

            builder.HasKey(c => c.Id);

            builder.Property<Guid>("ReviewId").IsRequired();
            builder.Property(c => c.AuthorId).IsRequired();
            builder.Property(c => c.Content).HasMaxLength(2000).IsRequired();
            builder.Property(c => c.CreatedAt).IsRequired();

            builder.HasOne<Review>()
                .WithMany()
                .HasForeignKey("ReviewId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
