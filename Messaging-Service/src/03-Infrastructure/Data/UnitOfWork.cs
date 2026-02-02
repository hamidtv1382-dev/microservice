using Messaging_Service.src._01_Domain.Core.Interfaces.Repositories;

namespace Messaging_Service.src._03_Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MessagingDbContext _context;

        public UnitOfWork(MessagingDbContext context)
        {
            _context = context;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.CommitAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
