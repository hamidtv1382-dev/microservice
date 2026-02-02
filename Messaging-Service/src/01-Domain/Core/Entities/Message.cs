using Messaging_Service.src._01_Domain.Core.Common;
using Messaging_Service.src._01_Domain.Core.Enums;
using Messaging_Service.src._01_Domain.Core.Events;
using Messaging_Service.src._01_Domain.Core.ValueObjects;

namespace Messaging_Service.src._01_Domain.Core.Entities
{
    public class Message : BaseEntity
    {
        public Guid ChatId { get; private set; }
        public Guid SenderId { get; private set; }

        public MessageContent Content { get; private set; }
        public MessageType Type { get; private set; }
        public MessageStatus Status { get; private set; }

        public AuditInfo Audit { get; private set; }

        public List<MessageAttachment> Attachments { get; private set; }
        public bool IsSensitive { get; private set; }

        private readonly List<IDomainEvent> _domainEvents;
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        // ✅ EF Core constructor
        protected Message()
        {
            Attachments = new List<MessageAttachment>();
            _domainEvents = new List<IDomainEvent>();
        }

        // ✅ Domain constructor
        public Message(
            Guid chatId,
            Guid senderId,
            MessageContent content,
            MessageType type,
            AuditInfo audit)
        {
            Id = Guid.NewGuid();
            ChatId = chatId;
            SenderId = senderId;
            Content = content;
            Type = type;
            Status = MessageStatus.Pending;
            Audit = audit;
            IsSensitive = false;

            Attachments = new List<MessageAttachment>();
            _domainEvents = new List<IDomainEvent>();

            AddDomainEvent(new MessageCreatedEvent(this));
        }

        public void MarkAsSensitive()
        {
            IsSensitive = true;
            Status = MessageStatus.PendingApproval;
        }

        public void Approve()
        {
            if (Status != MessageStatus.Pending &&
                Status != MessageStatus.PendingApproval)
                throw new InvalidOperationException("Message cannot be approved in its current state.");

            Status = MessageStatus.Approved;
            AddDomainEvent(new MessageApprovedEvent(this));
        }

        public void MarkAsSent()
        {
            if (Status != MessageStatus.Approved)
                throw new InvalidOperationException("Message must be approved before sending.");

            Status = MessageStatus.Sent;
            AddDomainEvent(new MessageSentEvent(this));
        }

        public void MarkAsFailed(string reason)
        {
            Status = MessageStatus.Failed;
        }

        public void AddAttachment(MessageAttachment attachment)
        {
            if (attachment is null)
                throw new ArgumentNullException(nameof(attachment));

            Attachments.Add(attachment);
        }

        protected void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents.Add(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
