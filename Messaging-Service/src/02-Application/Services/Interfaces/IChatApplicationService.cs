using Messaging_Service.src._02_Application.DTOs.Requests;
using Messaging_Service.src._02_Application.DTOs.Responses;

namespace Messaging_Service.src._02_Application.Services.Interfaces
{
    public interface IChatApplicationService
    {
        Task<Guid> CreateChatAsync(CreateChatRequestDto request);
        Task<IEnumerable<ChatSummaryResponseDto>> GetUserChatsAsync(Guid userId);
        Task<ChatSummaryResponseDto> GetChatByIdAsync(Guid chatId);
    }
}
