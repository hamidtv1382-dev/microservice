using Notification_Services.src._01_Domain.Services.Interfaces;

namespace Notification_Services.src._01_Domain.Services.Implementations
{
    public class NotificationDomainService : INotificationDomainService
    {
        public void ValidateNotificationContent(string title, string message)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Notification title cannot be empty.");

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Notification message cannot be empty.");
        }
    }
}

