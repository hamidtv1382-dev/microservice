using Messaging_Service.src._01_Domain.Core.Entities;

namespace Messaging_Service.src._01_Domain.Core.Events
{
    public class MessageApprovedEvent : IDomainEvent
    {
        public Message Message { get; }
        public DateTime OccurredOn { get; }

        public MessageApprovedEvent(Message message)
        {
            Message = message;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
