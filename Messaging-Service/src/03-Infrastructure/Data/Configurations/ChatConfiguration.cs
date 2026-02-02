using Messaging_Service.src._01_Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messaging_Service.src._03_Infrastructure.Data.Configurations
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.ToTable("Chats");
            builder.HasKey(x => x.Id);

            // BaseEntity fields
            builder.Property(x => x.CreatedDate)
                   .IsRequired();

            builder.Property(x => x.UpdatedDate);

            builder.Property(x => x.IsDeleted)
                   .IsRequired();

            // Normal properties
            builder.Property(x => x.Title)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(x => x.Type)
                   .IsRequired();

            // Relationships
            builder.HasMany(x => x.Participants)
                   .WithOne()
                   .HasForeignKey(x => x.ChatId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Messages)
                   .WithOne()
                   .HasForeignKey(x => x.ChatId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
