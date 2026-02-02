using Microsoft.EntityFrameworkCore;
using Notification_Services.src._01_Domain.Core.Aggregates.Notification;
using Notification_Services.src._01_Domain.Core.Enums;
using Notification_Services.src._01_Domain.Core.Interfaces.Repositories;
using Notification_Services.src._03_Infrastructure.Data;

namespace Notification_Services.src._03_Infrastructure.Repositories
{
    public class TemplateRepository : ITemplateRepository
    {
        private readonly AppDbContext _context;

        public TemplateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationTemplate?> GetByIdAsync(Guid id)
        {
            return await _context.NotificationTemplates.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }

        public async Task<IEnumerable<NotificationTemplate>> GetByTypeAsync(NotificationType type)
        {
            return await _context.NotificationTemplates
                .Where(t => t.Type == type && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(NotificationTemplate template)
        {
            await _context.NotificationTemplates.AddAsync(template);
        }

        public async Task UpdateAsync(NotificationTemplate template)
        {
            _context.NotificationTemplates.Update(template);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(NotificationTemplate template)
        {
            template.MarkAsDeleted();
            _context.NotificationTemplates.Update(template);
            await Task.CompletedTask;
        }
    }
}
