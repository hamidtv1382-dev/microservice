using Microsoft.EntityFrameworkCore;
using Notification_Services.src._01_Domain.Core.Aggregates.Notification;
using Notification_Services.src._01_Domain.Core.Enums;
using Notification_Services.src._01_Domain.Core.Interfaces.Repositories;
using Notification_Services.src._03_Infrastructure.Data;

namespace Notification_Services.src._03_Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Notification?> GetByIdAsync(Guid id)
        {
            return await _context.Notifications
                .Include(n => n.Recipients)
                .Include(n => n.Attachments)
                .FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted);
        }

        public async Task<IEnumerable<Notification>> GetPendingNotificationsAsync()
        {
            return await _context.Notifications
                .Include(n => n.Recipients)
                .Include(n => n.Attachments)
                .Where(n => n.Status == NotificationStatus.Pending && !n.ScheduledAt.HasValue || n.ScheduledAt <= DateTime.UtcNow)
                .Where(n => !n.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetByRecipientAsync(Guid recipientId)
        {
            return await _context.Notifications
                .Include(n => n.Recipients)
                .Include(n => n.Attachments)
                .Where(n => n.Recipients.Any(r => r.Id == recipientId) && !n.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }

        public async Task UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Notification notification)
        {
            notification.MarkAsDeleted();
            _context.Notifications.Update(notification);
            await Task.CompletedTask;
        }
    }
}
