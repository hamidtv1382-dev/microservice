using Notification_Services.src._02_Application.Interfaces;

namespace Notification_Services.src._03_Infrastructure.Services.External
{
    public class PushClient : IExternalMessagingGateway
    {
        public async Task<bool> SendEmailAsync(string to, string subject, string body, List<string> attachments = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendSmsAsync(string to, string message)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SendPushAsync(Guid userId, string title, string body)
        {
            // Simulate logic
            await Task.Delay(50);
            return true;
        }
    }
}
