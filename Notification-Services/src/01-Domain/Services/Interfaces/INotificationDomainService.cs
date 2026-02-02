namespace Notification_Services.src._01_Domain.Services.Interfaces
{
    public interface INotificationDomainService
    {
        void ValidateNotificationContent(string title, string message);
    }
}
