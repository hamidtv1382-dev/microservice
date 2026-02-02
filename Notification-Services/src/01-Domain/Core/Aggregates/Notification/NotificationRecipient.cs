using Notification_Services.src._01_Domain.Core.Enums;
using Notification_Services.src._01_Domain.Core.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notification_Services.src._01_Domain.Core.Aggregates.Notification
{
    [Table("NotificationRecipients")]
    public class NotificationRecipient
    {
        public Guid Id { get; private set; }
        public RecipientType Type { get; private set; }
        public string Contact { get; private set; }
        public bool IsRead { get; private set; }

        private NotificationRecipient() { }

        public NotificationRecipient(RecipientType type, RecipientContact contact)
        {
            Type = type;
            Contact = contact.Address;
            IsRead = false;
        }

        public void MarkAsRead() => IsRead = true;
    }
}