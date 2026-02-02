using Notification_Services.src._01_Domain.Core.Interfaces.Repositories;

namespace Notification_Services.src._01_Domain.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        INotificationRepository Notifications { get; }
        ITemplateRepository Templates { get; }
        Task<int> SaveChangesAsync();
    }
}
