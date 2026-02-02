using Seller_Finance_Service.src._02_Application.Interfaces;
using System.Text;
using System.Text.Json;

namespace Seller_Finance_Service.src._03_Infrastructure.Services.External
{
    public class NotificationServiceClient : INotificationService
    {
        private readonly HttpClient _httpClient;

        public NotificationServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendPayoutSuccessEmailAsync(Guid sellerId, decimal amount)
        {
            var payload = new { SellerId = sellerId, Amount = amount, Type = "PayoutSuccess" };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync("https://notification-service/api/emails/send", content);
        }

        public async Task SendPayoutFailureEmailAsync(Guid sellerId, string reason)
        {
            var payload = new { SellerId = sellerId, Reason = reason, Type = "PayoutFailure" };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync("https://notification-service/api/emails/send", content);
        }
    }
}
