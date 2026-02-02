using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification_Services.src._01_Domain.Core.Aggregates.Notification;

namespace Notification_Services.src._03_Infrastructure.Data.Configurations
{
    public class NotificationRecipientConfiguration : IEntityTypeConfiguration<NotificationRecipient>
    {
        public void Configure(EntityTypeBuilder<NotificationRecipient> builder)
        {
            builder.ToTable("NotificationRecipients");
            builder.HasKey(r => r.Id);
            builder.Property<Guid>("NotificationId").IsRequired();

            builder.Property(r => r.Type).HasConversion<int>().IsRequired();
            builder.Property(r => r.Contact).HasMaxLength(500).IsRequired();
            builder.Property(r => r.IsRead).IsRequired();

            builder.HasOne<Notification>()
                .WithMany(n => n.Recipients)
                .HasForeignKey("NotificationId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}