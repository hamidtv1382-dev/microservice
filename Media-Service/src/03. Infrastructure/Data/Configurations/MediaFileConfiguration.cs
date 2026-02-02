using Media_Service.src._01_Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Media_Service.src._03._Infrastructure.Data.Configurations
{
    public class MediaFileConfiguration : IEntityTypeConfiguration<MediaFile>
    {
        public void Configure(EntityTypeBuilder<MediaFile> builder)
        {
            builder.ToTable("MediaFiles");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.OriginalFileName).HasMaxLength(255).IsRequired();
            builder.Property(m => m.StoredFileName).HasMaxLength(255).IsRequired();
            builder.Property(m => m.Extension).HasMaxLength(20).IsRequired();
            builder.Property(m => m.SizeBytes).IsRequired();

            builder.Property(m => m.RelativePath).HasMaxLength(1000).IsRequired();
            builder.Property(m => m.AbsoluteUrl).HasMaxLength(2000).IsRequired();

            builder.Property(m => m.Type).HasConversion<int>().IsRequired();
            builder.Property(m => m.OwnerType).HasConversion<int>().IsRequired();
            builder.Property(m => m.OwnerId).IsRequired();

            builder.Property(m => m.Visibility).HasConversion<int>().IsRequired();

            builder.OwnsOne(m => m.Audit, audit =>
            {
                audit.Property(a => a.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(100);
                audit.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
                audit.Property(a => a.LastModifiedBy).HasColumnName("LastModifiedBy").HasMaxLength(100);
                audit.Property(a => a.LastModifiedAt).HasColumnName("LastModifiedAt");
            });

            builder.Property(m => m.CreatedAt).IsRequired();
            builder.Property(m => m.UpdatedAt);
            builder.Property(m => m.IsDeleted).HasDefaultValue(false);
        }
    }
}
