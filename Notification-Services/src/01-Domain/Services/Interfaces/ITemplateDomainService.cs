using Notification_Services.src._01_Domain.Core.Aggregates.Notification;
using Notification_Services.src._01_Domain.Core.Enums;

namespace Notification_Services.src._01_Domain.Services.Interfaces
{
    public interface ITemplateDomainService
    {
        Task<NotificationTemplate?> GetTemplateForTypeAndLanguageAsync(NotificationType type, string languageCode);
    }
}
