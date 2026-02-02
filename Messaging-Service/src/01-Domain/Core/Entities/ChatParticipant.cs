using Messaging_Service.src._01_Domain.Core.Common;
using Messaging_Service.src._01_Domain.Core.Enums;

namespace Messaging_Service.src._01_Domain.Core.Entities
{
    public class ChatParticipant : BaseEntity
    {
        public Guid ChatId { get; private set; }
        public Guid UserId { get; private set; }
        public RecipientType Role { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? LastReadAt { get; private set; }

        protected ChatParticipant() { }

        public ChatParticipant(Guid chatId, Guid userId, RecipientType role)
        {
            Id = Guid.NewGuid();
            ChatId = chatId;
            UserId = userId;
            Role = role;
            IsActive = true;
        }

        public void Leave()
        {
            IsActive = false;
        }

        public void MarkAsRead()
        {
            LastReadAt = DateTime.UtcNow;
        }
    }
}
