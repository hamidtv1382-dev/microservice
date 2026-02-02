using Media_Service.src._01_Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Media_Service.src._03._Infrastructure.Data.Configurations
{
    public class MediaFolderConfiguration : IEntityTypeConfiguration<MediaFolder>
    {
        public void Configure(EntityTypeBuilder<MediaFolder> builder)
        {
            builder.ToTable("MediaFolders");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.FolderName).HasMaxLength(255).IsRequired();
            builder.Property(m => m.FullPhysicalPath).HasMaxLength(1000).IsRequired();

            builder.Property(m => m.OwnerType).HasConversion<int>().IsRequired();

            builder.Property(m => m.OwnerId).IsRequired(false);
            builder.Property(m => m.ParentFolderId).IsRequired(false);

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
