using Messaging_Service.src._01_Domain.Core.Entities;

namespace Messaging_Service.src._01_Domain.Core.Events
{
    public class MessageCreatedEvent : IDomainEvent
    {
        public Message Message { get; }
        public DateTime OccurredOn { get; }

        public MessageCreatedEvent(Message message)
        {
            Message = message;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
