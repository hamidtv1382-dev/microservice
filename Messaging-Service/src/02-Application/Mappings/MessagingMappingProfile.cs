using Messaging_Service.src._01_Domain.Core.Entities;
using Messaging_Service.src._02_Application.DTOs.Requests;
using Messaging_Service.src._02_Application.DTOs.Responses;

namespace Messaging_Service.src._02_Application.Mappings
{
    public static class MessagingMappingProfile
    {
        public static MessageResponseDto MapMessageToDto(Message message)
        {
            if (message == null)
                return null;

            return new MessageResponseDto
            {
                Id = message.Id,
                ChatId = message.ChatId,
                SenderId = message.SenderId,

                // ✅ null-safe ValueObject mapping
                Content = message.Content?.Text ?? string.Empty,

                Status = message.Status.ToString(),
                CreatedDate = message.CreatedDate,

                // ✅ null-safe collection mapping
                Attachments = message.Attachments == null
                    ? new List<MessageAttachmentDto>()
                    : message.Attachments.Select(a => new MessageAttachmentDto
                    {
                        FileName = a.FileName,
                        FilePath = a.FilePath,
                        FileSize = a.FileSize,
                        MimeType = a.MimeType
                    }).ToList()
            };
        }

        public static List<MessageResponseDto> MapMessagesToDtos(IEnumerable<Message> messages)
        {
            if (messages == null)
                return new List<MessageResponseDto>();

            return messages
                .Select(MapMessageToDto)
                .Where(x => x != null)
                .ToList();
        }

        public static ChatSummaryResponseDto MapChatToDto(Chat chat)
        {
            if (chat == null)
                return null;

            return new ChatSummaryResponseDto
            {
                Id = chat.Id,
                Title = chat.Title,
                CreatedDate = chat.CreatedDate,
                ParticipantIds = chat.Participants?
                    .Where(p => p.IsActive)
                    .Select(p => p.UserId)
                    .ToList() ?? new List<Guid>()
            };
        }

        public static List<ChatSummaryResponseDto> MapChatsToDtos(IEnumerable<Chat> chats)
        {
            if (chats == null)
                return new List<ChatSummaryResponseDto>();

            return chats
                .Select(MapChatToDto)
                .Where(x => x != null)
                .ToList();
        }
    }
}
