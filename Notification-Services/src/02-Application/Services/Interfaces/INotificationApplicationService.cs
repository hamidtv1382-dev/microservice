using Notification_Services.src._02_Application.DTOs.Requests;
using Notification_Services.src._02_Application.DTOs.Responses;

namespace Notification_Services.src._02_Application.Services.Interfaces
{
    public interface INotificationApplicationService
    {
        Task<NotificationResponseDto> CreateNotificationAsync(CreateNotificationRequestDto request);
        Task<NotificationResponseDto> GetNotificationAsync(Guid id);
        Task<NotificationStatusResponseDto> SendNotificationAsync(Guid id);
        Task<List<NotificationResponseDto>> GetPendingNotificationsAsync();
        Task<List<NotificationResponseDto>> GetNotificationsByRecipientAsync(Guid recipientId);
    }
}
