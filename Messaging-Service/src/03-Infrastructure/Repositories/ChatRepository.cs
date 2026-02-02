using Messaging_Service.src._01_Domain.Core.Entities;
using Messaging_Service.src._01_Domain.Core.Enums;
using Messaging_Service.src._01_Domain.Core.Interfaces.Repositories;
using Messaging_Service.src._03_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Messaging_Service.src._03_Infrastructure.Repositories
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        public ChatRepository(MessagingDbContext context) : base(context)
        {
        }

        public async Task<Chat> GetChatByParticipantsAsync(Guid user1Id, Guid user2Id, ChatType type)
        {
            return await _context.Chats
                .Where(c => c.Type == type)
                .Where(c => c.Participants.Any(p => p.UserId == user1Id && p.IsActive))
                .Where(c => c.Participants.Any(p => p.UserId == user2Id && p.IsActive))
                .FirstOrDefaultAsync();
        }

        public async Task<Chat> GetChatWithParticipantsAsync(Guid chatId)
        {
            return await _context.Chats
                .Include(x => x.Participants)
                .FirstOrDefaultAsync(x => x.Id == chatId);
        }

        public async Task<IEnumerable<Chat>> GetUserChatsAsync(Guid userId)
        {
            return await _context.Chats
                .Include(x => x.Participants)
                .Include(x => x.Messages.OrderByDescending(m => m.CreatedDate).Take(1))
                .Where(x => x.Participants.Any(p => p.UserId == userId && p.IsActive))
                .OrderByDescending(x => x.Messages.Any() ? x.Messages.Max(m => m.CreatedDate) : x.CreatedDate)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
