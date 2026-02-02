using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification_Services.src._01_Domain.Core.Aggregates.Notification;

namespace Notification_Services.src._03_Infrastructure.Data.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Id).HasDefaultValueSql("newid()").IsRequired();

            builder.Property(n => n.Type).HasConversion<int>().IsRequired();
            builder.Property(n => n.Status).HasConversion<int>().IsRequired();
            builder.Property(n => n.Title).HasMaxLength(200).IsRequired();
            builder.Property(n => n.Message).HasMaxLength(5000).IsRequired();
            builder.Property(n => n.ScheduledAt);
            builder.Property(n => n.SentAt);
            builder.Property(n => n.ErrorMessage).HasMaxLength(1000);
            builder.Property(n => n.CreatedAt).IsRequired();
            builder.Property(n => n.UpdatedAt);
            builder.Property(n => n.IsDeleted).HasDefaultValue(false);

            builder.Metadata.FindNavigation(nameof(Notification.Recipients))!.SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Notification.Attachments))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}