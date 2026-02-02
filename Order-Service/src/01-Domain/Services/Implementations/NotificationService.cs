using Order_Service.src._01_Domain.Services.Interfaces;

namespace Order_Service.src._01_Domain.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly ILoggingService _loggingService;

        public NotificationService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public async Task SendOrderCreatedNotificationAsync(Guid userId, Guid orderId, string userEmail)
        {
            // Simulate sending an email/SMS
            await Task.Run(() =>
            {
                _loggingService.LogInformation($"Sending Order Created notification to User {userEmail} for Order {orderId}.");
                // Actual implementation would use an SMTP client or external service like SendGrid/AWS SES
            });
        }

        public async Task SendOrderStatusChangedNotificationAsync(Guid userId, Guid orderId, string status, string userEmail)
        {
            await Task.Run(() =>
            {
                _loggingService.LogInformation($"Sending Order Status Changed notification to User {userEmail}. Order {orderId} is now {status}.");
            });
        }

        public async Task SendPaymentFailedNotificationAsync(Guid userId, Guid orderId, string reason, string userEmail)
        {
            await Task.Run(() =>
            {
                _loggingService.LogInformation($"Sending Payment Failed notification to User {userEmail}. Order {orderId}. Reason: {reason}");
            });
        }
    }
}
