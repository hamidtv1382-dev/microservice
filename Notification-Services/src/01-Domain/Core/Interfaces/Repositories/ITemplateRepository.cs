using Notification_Services.src._01_Domain.Core.Aggregates.Notification;
using Notification_Services.src._01_Domain.Core.Enums;

namespace Notification_Services.src._01_Domain.Core.Interfaces.Repositories
{
    public interface ITemplateRepository
    {
        Task<NotificationTemplate?> GetByIdAsync(Guid id);
        Task<IEnumerable<NotificationTemplate>> GetByTypeAsync(NotificationType type);
        Task AddAsync(NotificationTemplate template);
        Task UpdateAsync(NotificationTemplate template);
        Task DeleteAsync(NotificationTemplate template);
    }
}
