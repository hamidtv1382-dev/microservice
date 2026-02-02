using Messaging_Service.src._01_Domain.Core.Entities;
using Messaging_Service.src._01_Domain.Core.Enums;
using Messaging_Service.src._01_Domain.Core.Interfaces.Repositories;
using Messaging_Service.src._01_Domain.Core.Interfaces.Services;
using Messaging_Service.src._01_Domain.Core.ValueObjects;
using Messaging_Service.src._02_Application.DTOs.Requests;
using Messaging_Service.src._02_Application.DTOs.Responses;
using Messaging_Service.src._02_Application.Exceptions;
using Messaging_Service.src._02_Application.Hubs;
using Messaging_Service.src._02_Application.Mappings;
using Messaging_Service.src._02_Application.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Messaging_Service.src._02_Application.Services.Implementations
{
    public class MessageApplicationService : IMessageApplicationService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IModerationService _moderationService;
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<MessagingHub> _hubContext; // <--- تزریق هاب

        public MessageApplicationService(IMessageRepository messageRepository, IChatRepository chatRepository, IModerationService moderationService, INotificationService notificationService, IUnitOfWork unitOfWork, IHubContext<MessagingHub> hubContext)
        {
            _messageRepository = messageRepository;
            _chatRepository = chatRepository;
            _moderationService = moderationService;
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext; // <--- مقداردهی هاب
        }

        public async Task<Guid> SendMessageAsync(SendMessageRequestDto request)
        {
            var chat = await _chatRepository.GetChatWithParticipantsAsync(request.ChatId);
            if (chat == null) throw new ChatNotFoundException(request.ChatId);

            var content = new MessageContent(request.Content);
            var audit = new AuditInfo(request.IpAddress, request.UserAgent);
            var messageType = request.Attachments != null && request.Attachments.Any() ? MessageType.File : MessageType.Text;

            var message = new Message(request.ChatId, request.SenderId, content, messageType, audit);

            if (await _moderationService.ValidateContentAsync(request.Content, request.SenderId) == false)
            {
                message.MarkAsSensitive();
            }
            else
            {
                message.Approve();
            }

            await _messageRepository.AddAsync(message);
            await _unitOfWork.CommitAsync();

            if (message.Status == MessageStatus.Approved)
            {
                // ساخت آبجکت پیام برای ارسال به اپلیکیشن
                var messageDto = new
                {
                    Id = message.Id,
                    ChatId = request.ChatId,
                    SenderId = request.SenderId,
                    Content = request.Content,
                    CreatedDate = message.CreatedDate
                };

                // --- ارسال پیام آنی به گروه (SignalR) ---
                // همه کسانی که در این چت متصل هستند پیام را دریافت می‌کنند
                await _hubContext.Clients.Group(request.ChatId.ToString()).SendAsync("ReceiveMessage", messageDto);

                var recipients = chat.Participants.Where(p => p.UserId != request.SenderId && p.IsActive);
                foreach (var recipient in recipients)
                {
                    // ارسال نوتیفیکیشن گوشی (Firebase)
                    await _notificationService.SendPushAsync(recipient.UserId, "New Message", request.Content);
                }

                message.MarkAsSent();
                await _messageRepository.UpdateAsync(message);
                await _unitOfWork.CommitAsync();
            }

            return message.Id;
        }

        public async Task ApproveMessageAsync(Guid messageId)
        {
            var message = await _messageRepository.GetByIdAsync(messageId);
            if (message == null) throw new MessageNotFoundException(messageId);

            message.Approve();
            await _messageRepository.UpdateAsync(message);
            await _unitOfWork.CommitAsync();

            var chat = await _chatRepository.GetChatWithParticipantsAsync(message.ChatId);
            if (chat != null)
            {
                // ساخت آبجکت پیام
                var messageDto = new
                {
                    Id = message.Id,
                    ChatId = message.ChatId,
                    SenderId = message.SenderId,
                    Content = message.Content.Text,
                    CreatedDate = message.CreatedDate
                };

                // --- ارسال پیام آنی ---
                await _hubContext.Clients.Group(message.ChatId.ToString()).SendAsync("ReceiveMessage", messageDto);

                var recipients = chat.Participants.Where(p => p.UserId != message.SenderId && p.IsActive);
                foreach (var recipient in recipients)
                {
                    await _notificationService.SendPushAsync(recipient.UserId, "New Message", message.Content.Text);
                }

                message.MarkAsSent();
                await _messageRepository.UpdateAsync(message);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<IEnumerable<MessageResponseDto>> GetChatMessagesAsync(Guid chatId, int page, int pageSize)
        {
            var messages = await _messageRepository.GetMessagesByChatIdAsync(chatId, page, pageSize);
            return MessagingMappingProfile.MapMessagesToDtos(messages);
        }
    }
}
