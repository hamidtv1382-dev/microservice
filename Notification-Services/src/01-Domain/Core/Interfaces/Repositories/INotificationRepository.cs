using Notification_Services.src._01_Domain.Core.Aggregates.Notification;

namespace Notification_Services.src._01_Domain.Core.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification?> GetByIdAsync(Guid id);
        Task<IEnumerable<Notification>> GetPendingNotificationsAsync();
        Task<IEnumerable<Notification>> GetByRecipientAsync(Guid recipientId);
        Task AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task DeleteAsync(Notification notification);
    }
}
