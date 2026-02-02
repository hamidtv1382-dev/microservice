using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Review_Rating_Service.src._01_Domain.Core.Aggregates.Review;

namespace Review_Rating_Service.src._03_Infrastructure.Data.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.ToTable("Ratings");

            builder.HasKey(r => r.Id);

            builder.Property<Guid>("ReviewId").IsRequired();

            // تنظیم Type که Enum است
            builder.Property(r => r.Type).HasConversion<int>().IsRequired();

            // تنظیم Value که int است (چون Flattened شده دیگر ValueObject نیست)
            builder.Property(r => r.Value).IsRequired();

            // تنظیم رابطه با Review
            builder.HasOne(r => r.Review)
           .WithMany()
           .HasForeignKey(r => r.ReviewId)
           .OnDelete(DeleteBehavior.Restrict); // حذف Cascade حذف چند مسیر
            ; // حذف Cascade حذف چند مسیر

        }
    }
}
