using Messaging_Service.src._01_Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messaging_Service.src._03_Infrastructure.Data.Configurations
{
    public class ChatParticipantConfiguration : IEntityTypeConfiguration<ChatParticipant>
    {
        public void Configure(EntityTypeBuilder<ChatParticipant> builder)
        {
            builder.ToTable("ChatParticipants");
            builder.HasKey(x => x.Id);

            // BaseEntity fields
            builder.Property(x => x.CreatedDate)
                   .IsRequired();

            builder.Property(x => x.UpdatedDate);

            builder.Property(x => x.IsDeleted)
                   .IsRequired();

            // Normal properties
            builder.Property(x => x.Role).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();

            // Unique index
            builder.HasIndex(x => new { x.ChatId, x.UserId }).IsUnique();
        }
    }
}
