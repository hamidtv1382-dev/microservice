using Notification_Services.src._01_Domain.Core.Common;
using Notification_Services.src._01_Domain.Core.Enums;

namespace Notification_Services.src._01_Domain.Core.Aggregates.Notification
{
    public class NotificationTemplate : Entity
    {
        public string Name { get; private set; }
        public string Subject { get; private set; }
        public string BodyContent { get; private set; }
        public NotificationType Type { get; private set; }
        public string LanguageCode { get; private set; }

        public NotificationTemplate(string name, string subject, string bodyContent, NotificationType type, string languageCode = "en")
        {
            Name = name;
            Subject = subject;
            BodyContent = bodyContent;
            Type = type;
            LanguageCode = languageCode;
        }

        public void UpdateContent(string subject, string bodyContent)
        {
            Subject = subject;
            BodyContent = bodyContent;
            SetUpdatedAt();
        }
    }
}