using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Review_Rating_Service.src._01_Domain.Core.Aggregates.Review;

namespace Review_Rating_Service.src._03_Infrastructure.Data.Configurations
{
    public class ReviewAttachmentConfiguration : IEntityTypeConfiguration<ReviewAttachment>
    {
        public void Configure(EntityTypeBuilder<ReviewAttachment> builder)
        {
            builder.ToTable("ReviewAttachments");

            builder.HasKey(a => a.Id);

            builder.Property<Guid>("ReviewId").IsRequired();
            builder.Property(a => a.Url).IsRequired();
            builder.Property(a => a.Type).HasConversion<int>().IsRequired();

            builder.HasOne(a => a.Review)
          .WithMany()
          .HasForeignKey(a => a.ReviewId)
          .OnDelete(DeleteBehavior.Restrict); // حذف Cascade حذف چند مسیر

        }
    }
}
