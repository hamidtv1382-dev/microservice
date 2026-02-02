using Notification_Services.src._01_Domain.Core.Interfaces.Repositories;
using Notification_Services.src._01_Domain.Core.Interfaces.UnitOfWork;
using Notification_Services.src._03_Infrastructure.Repositories;

namespace Notification_Services.src._03_Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private INotificationRepository _notificationRepository;
        private ITemplateRepository _templateRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public INotificationRepository Notifications
        {
            get
            {
                if (_notificationRepository == null)
                    _notificationRepository = new NotificationRepository(_context);
                return _notificationRepository;
            }
        }

        public ITemplateRepository Templates
        {
            get
            {
                if (_templateRepository == null)
                    _templateRepository = new TemplateRepository(_context);
                return _templateRepository;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
