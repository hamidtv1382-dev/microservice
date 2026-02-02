using Messaging_Service.src._01_Domain.Core.Entities;

namespace Messaging_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<IEnumerable<Message>> GetMessagesByChatIdAsync(Guid chatId, int page, int pageSize);
        Task<Message> GetMessageWithAttachmentsAsync(Guid messageId);
    }
}
