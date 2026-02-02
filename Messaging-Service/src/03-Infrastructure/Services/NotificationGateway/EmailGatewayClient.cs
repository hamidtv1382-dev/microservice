using Messaging_Service.src._01_Domain.Core.Interfaces.Services;

namespace Messaging_Service.src._03_Infrastructure.Services.NotificationGateway
{
    public class EmailGatewayClient : INotificationService
    {
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            await Task.Run(() =>
            {
                Console.WriteLine($"[Email Gateway] Sending to {email}: {subject} | {body}");
            });
        }

        public Task SendSmsAsync(string phoneNumber, string message)
        {
            throw new NotSupportedException("EmailGatewayClient does not support SMS.");
        }

        public Task SendPushAsync(Guid userId, string title, string message)
        {
            throw new NotSupportedException("EmailGatewayClient does not support Push.");
        }
    }
}
