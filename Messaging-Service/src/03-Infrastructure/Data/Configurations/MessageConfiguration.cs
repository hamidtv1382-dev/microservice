using Messaging_Service.src._01_Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messaging_Service.src._03_Infrastructure.Data.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");
            builder.HasKey(x => x.Id);

            // BaseEntity fields
            builder.Property(x => x.CreatedDate)
                   .IsRequired();

            builder.Property(x => x.UpdatedDate);

            builder.Property(x => x.IsDeleted)
                   .IsRequired();

            // Value Objects
            builder.OwnsOne(x => x.Content, navigation =>
            {
                navigation.Property(p => p.Text)
                          .HasColumnName("ContentText")
                          .IsRequired()
                          .HasMaxLength(2000);
            });

            builder.OwnsOne(x => x.Audit, navigation =>
            {
                navigation.Property(p => p.IpAddress)
                          .HasColumnName("IpAddress")
                          .HasMaxLength(50);
                navigation.Property(p => p.UserAgent)
                          .HasColumnName("UserAgent")
                          .HasMaxLength(500);
            });

            // Normal properties
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.IsSensitive).IsRequired();
            builder.Property(x => x.ChatId).IsRequired();
            builder.Property(x => x.SenderId).IsRequired();

            // Relationships
            builder.HasOne<Chat>()
                   .WithMany()
                   .HasForeignKey(x => x.ChatId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Attachments)
                   .WithOne()
                   .HasForeignKey(x => x.MessageId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Ignore internal properties
            builder.Ignore(x => x.DomainEvents);
        }
    }
}
