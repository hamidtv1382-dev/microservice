using Messaging_Service.src._01_Domain.Core.Entities;
using Messaging_Service.src._01_Domain.Core.Interfaces.Repositories;
using Messaging_Service.src._03_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Messaging_Service.src._03_Infrastructure.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(MessagingDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Message>> GetMessagesByChatIdAsync(Guid chatId, int page, int pageSize)
        {
            return await _context.Messages
                .Where(x => x.ChatId == chatId)
                .Include(x => x.Attachments)
                .OrderByDescending(x => x.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Message> GetMessageWithAttachmentsAsync(Guid messageId)
        {
            return await _context.Messages
                .Include(x => x.Attachments)
                .FirstOrDefaultAsync(x => x.Id == messageId);
        }
    }
}
