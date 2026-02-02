using Messaging_Service.src._01_Domain.Core.Common;
using Messaging_Service.src._01_Domain.Core.Enums;

namespace Messaging_Service.src._01_Domain.Core.Entities
{
    public class Chat : BaseEntity
    {
        public string Title { get; private set; }
        public ChatType Type { get; private set; }
        public Guid ReferenceId { get; private set; } // OrderId, TicketId, etc.
        public ICollection<ChatParticipant> Participants { get; private set; }
        public ICollection<Message> Messages { get; private set; }

        protected Chat()
        {
            Participants = new HashSet<ChatParticipant>();
            Messages = new HashSet<Message>();
        }

        public Chat(string title, ChatType type, Guid referenceId)
        {
            Id = Guid.NewGuid();
            Title = title;
            Type = type;
            ReferenceId = referenceId;
            Participants = new HashSet<ChatParticipant>();
            Messages = new HashSet<Message>();
        }

        public void AddParticipant(Guid userId, RecipientType role)
        {
            if (Participants.Any(p => p.UserId == userId)) return;

            var participant = new ChatParticipant(Id, userId, role);
            Participants.Add(participant);
        }

        public void RemoveParticipant(Guid userId)
        {
            var participant = Participants.FirstOrDefault(p => p.UserId == userId);
            if (participant != null)
            {
                participant.Leave();
            }
        }
    }
}
