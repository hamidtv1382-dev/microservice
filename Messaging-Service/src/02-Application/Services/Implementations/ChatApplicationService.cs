using Messaging_Service.src._01_Domain.Core.Entities;
using Messaging_Service.src._01_Domain.Core.Enums;
using Messaging_Service.src._01_Domain.Core.Interfaces.Repositories;
using Messaging_Service.src._02_Application.DTOs.Requests;
using Messaging_Service.src._02_Application.DTOs.Responses;
using Messaging_Service.src._02_Application.Exceptions;
using Messaging_Service.src._02_Application.Mappings;
using Messaging_Service.src._02_Application.Services.Interfaces;

namespace Messaging_Service.src._02_Application.Services.Implementations
{
    public class ChatApplicationService : IChatApplicationService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChatApplicationService(IChatRepository chatRepository, IUnitOfWork unitOfWork)
        {
            _chatRepository = chatRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> CreateChatAsync(CreateChatRequestDto request)
        {
            var existingChat = await _chatRepository.GetChatByParticipantsAsync(request.CreatorId, request.ParticipantId, request.Type);
            if (existingChat != null) return existingChat.Id;

            var chat = new Chat(request.Title, request.Type, request.ReferenceId);
            chat.AddParticipant(request.CreatorId, RecipientType.User); // Assuming Creator is User for simplicity
            chat.AddParticipant(request.ParticipantId, request.ParticipantRole);

            await _chatRepository.AddAsync(chat);
            await _unitOfWork.CommitAsync(); // اصلاح شد: SaveChangesAsync به CommitAsync تغییر یافت

            return chat.Id;
        }

        public async Task<IEnumerable<ChatSummaryResponseDto>> GetUserChatsAsync(Guid userId)
        {
            var chats = await _chatRepository.GetUserChatsAsync(userId);
            return MessagingMappingProfile.MapChatsToDtos(chats);
        }

        public async Task<ChatSummaryResponseDto> GetChatByIdAsync(Guid chatId)
        {
            var chat = await _chatRepository.GetChatWithParticipantsAsync(chatId);
            if (chat == null) throw new ChatNotFoundException(chatId);

            return MessagingMappingProfile.MapChatToDto(chat);
        }
    }
}
