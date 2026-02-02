using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification_Services.src._01_Domain.Core.Aggregates.Notification;

namespace Notification_Services.src._03_Infrastructure.Data.Configurations
{
    public class NotificationAttachmentConfiguration : IEntityTypeConfiguration<NotificationAttachment>
    {
        public void Configure(EntityTypeBuilder<NotificationAttachment> builder)
        {
            builder.ToTable("NotificationAttachments");
            builder.HasKey(a => a.Id);
            builder.Property<Guid>("NotificationId").IsRequired();

            builder.Property(a => a.FileName).HasMaxLength(255).IsRequired();
            builder.Property(a => a.FileUrl).HasMaxLength(1000).IsRequired();
            builder.Property(a => a.FileSizeBytes).IsRequired();

            builder.HasOne<Notification>()
                .WithMany(n => n.Attachments)
                .HasForeignKey("NotificationId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}