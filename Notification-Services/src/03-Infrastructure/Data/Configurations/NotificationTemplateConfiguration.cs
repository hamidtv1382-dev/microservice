using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification_Services.src._01_Domain.Core.Aggregates.Notification;

namespace Notification_Services.src._03_Infrastructure.Data.Configurations
{
    public class NotificationTemplateConfiguration : IEntityTypeConfiguration<NotificationTemplate>
    {
        public void Configure(EntityTypeBuilder<NotificationTemplate> builder)
        {
            builder.ToTable("NotificationTemplates");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).HasMaxLength(100).IsRequired();
            builder.Property(t => t.Subject).HasMaxLength(200).IsRequired();
            builder.Property(t => t.BodyContent).IsRequired();
            builder.Property(t => t.Type).HasConversion<int>().IsRequired();
            builder.Property(t => t.LanguageCode).HasMaxLength(10).IsRequired();
            builder.Property(t => t.CreatedAt).IsRequired();
            builder.Property(t => t.UpdatedAt);
            builder.Property(t => t.IsDeleted).HasDefaultValue(false);
        }
    }
}