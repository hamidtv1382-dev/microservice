using Messaging_Service.src._02_Application.DTOs.Requests;
using Messaging_Service.src._02_Application.DTOs.Responses;

namespace Messaging_Service.src._02_Application.Services.Interfaces
{
    public interface IMessageApplicationService
    {
        Task<Guid> SendMessageAsync(SendMessageRequestDto request);
        Task ApproveMessageAsync(Guid messageId);
        Task<IEnumerable<MessageResponseDto>> GetChatMessagesAsync(Guid chatId, int page, int pageSize);
    }
}
