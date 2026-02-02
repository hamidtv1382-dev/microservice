using Notification_Services.src._02_Application.Interfaces;

namespace Notification_Services.src._03_Infrastructure.Services.External
{
    public class EmailClient : IExternalMessagingGateway
    {
        private readonly HttpClient _httpClient;

        public EmailClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body, List<string> attachments = null)
        {
            var payload = new { To = to, Subject = subject, Body = body, Attachments = attachments };
            var response = await _httpClient.PostAsJsonAsync("/api/email/send", payload);
            return response.IsSuccessStatusCode;
        }

        public Task<bool> SendSmsAsync(string to, string message)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendPushAsync(Guid userId, string title, string body)
        {
            throw new NotImplementedException();
        }
    }
}
