using Notification_Services.src._02_Application.Interfaces;

namespace Notification_Services.src._03_Infrastructure.Services.External
{
    public class SmsClient : IExternalMessagingGateway
    {
        public async Task<bool> SendEmailAsync(string to, string subject, string body, List<string> attachments = null)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SendSmsAsync(string to, string message)
        {
            var payload = new { To = to, Message = message };
            // Simulated HTTP call
            await Task.Delay(100);
            return true;
        }

        public Task<bool> SendPushAsync(Guid userId, string title, string body)
        {
            throw new NotImplementedException();
        }
    }
}
