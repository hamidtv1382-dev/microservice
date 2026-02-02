using Messaging_Service.src._01_Domain.Core.Entities;
using Messaging_Service.src._01_Domain.Core.Enums;

namespace Messaging_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface IChatRepository : IRepository<Chat>
    {
        Task<Chat> GetChatByParticipantsAsync(Guid user1Id, Guid user2Id, ChatType type);
        Task<Chat> GetChatWithParticipantsAsync(Guid chatId);
        Task<IEnumerable<Chat>> GetUserChatsAsync(Guid userId);
    }
}
