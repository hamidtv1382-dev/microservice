using AutoMapper;
using Notification_Services.src._01_Domain.Core.Aggregates.Notification;
using Notification_Services.src._01_Domain.Core.Enums;
using Notification_Services.src._01_Domain.Core.Interfaces.UnitOfWork;
using Notification_Services.src._01_Domain.Core.ValueObjects;
using Notification_Services.src._01_Domain.Services.Interfaces;
using Notification_Services.src._02_Application.DTOs.Requests;
using Notification_Services.src._02_Application.DTOs.Responses;
using Notification_Services.src._02_Application.Exceptions;
using Notification_Services.src._02_Application.Interfaces;
using Notification_Services.src._02_Application.Services.Interfaces;

namespace Notification_Services.src._02_Application.Services.Implementations
{
    public class NotificationApplicationService : INotificationApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationDomainService _domainService;
        private readonly ITemplateDomainService _templateDomainService;
        private readonly IExternalMessagingGateway _messagingGateway;
        private readonly IMapper _mapper;

        public NotificationApplicationService(IUnitOfWork unitOfWork, INotificationDomainService domainService, ITemplateDomainService templateDomainService, IExternalMessagingGateway messagingGateway, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _domainService = domainService;
            _templateDomainService = templateDomainService;
            _messagingGateway = messagingGateway;
            _mapper = mapper;
        }

        public async Task<NotificationResponseDto> CreateNotificationAsync(CreateNotificationRequestDto request)
        {
            // 1️⃣ اعتبارسنجی
            _domainService.ValidateNotificationContent(request.Title, request.Message);

            // 2️⃣ ساخت Notification اصلی
            var notification = new Notification(
                request.Type,
                new NotificationTitle(request.Title),
                new NotificationMessage(request.Message)
            );

            // 3️⃣ افزودن Recipients به Aggregate
            foreach (var recipientDto in request.Recipients)
            {
                var recipient = new NotificationRecipient(
                    recipientDto.Type,
                    new RecipientContact(recipientDto.Contact)
                );

                notification.AddRecipient(recipient); // فقط به Aggregate اضافه می‌کنیم
            }

            // 4️⃣ افزودن Attachments به Aggregate
            foreach (var url in request.AttachmentUrls)
            {
                var attachment = new NotificationAttachment("attachment", url, 0);
                notification.AddAttachment(attachment);
            }

            // 5️⃣ Add Notification به Repository
            await _unitOfWork.Notifications.AddAsync(notification);

            // 6️⃣ Save نهایی: EF Core با Cascade همه Recipients و Attachments را Insert می‌کند
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<NotificationResponseDto>(notification);
        }


        public async Task<NotificationResponseDto> GetNotificationAsync(Guid id)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(id);
            if (notification == null)
            {
                throw new NotificationFailedException("Notification not found.");
            }

            return _mapper.Map<NotificationResponseDto>(notification);
        }

        public async Task<NotificationStatusResponseDto> SendNotificationAsync(Guid id)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(id);
            if (notification == null) throw new NotificationFailedException("Notification not found.");

            bool success = false;
            try
            {
                if (notification.Type == NotificationType.Email)
                {
                    // Assuming first recipient is target
                    var recipient = notification.Recipients.FirstOrDefault();
                    if (recipient == null) throw new InvalidRecipientException("No recipient defined.");

                    // اصلاح شده: چون Contact از نوع string است
                    success = await _messagingGateway.SendEmailAsync(recipient.Contact, notification.Title, notification.Message);
                }
                else if (notification.Type == NotificationType.SMS)
                {
                    var recipient = notification.Recipients.FirstOrDefault();
                    if (recipient == null) throw new InvalidRecipientException("No recipient defined.");

                    // اصلاح شده: چون Contact از نوع string است
                    success = await _messagingGateway.SendSmsAsync(recipient.Contact, notification.Message);
                }
                else if (notification.Type == NotificationType.Push)
                {
                    // Simulate user ID extraction
                    success = await _messagingGateway.SendPushAsync(Guid.NewGuid(), notification.Title, notification.Message);
                }

                if (success)
                {
                    notification.MarkAsSent();
                }
                else
                {
                    notification.MarkAsFailed("Gateway returned false.");
                }
            }
            catch (Exception ex)
            {
                notification.MarkAsFailed(ex.Message);
            }

            await _unitOfWork.Notifications.UpdateAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<NotificationStatusResponseDto>(notification);
        }

        public async Task<List<NotificationResponseDto>> GetPendingNotificationsAsync()
        {
            var notifications = await _unitOfWork.Notifications.GetPendingNotificationsAsync();
            return _mapper.Map<List<NotificationResponseDto>>(notifications);
        }

        public async Task<List<NotificationResponseDto>> GetNotificationsByRecipientAsync(Guid recipientId)
        {
            var notifications = await _unitOfWork.Notifications.GetByRecipientAsync(recipientId);
            return _mapper.Map<List<NotificationResponseDto>>(notifications);
        }
    }
}
