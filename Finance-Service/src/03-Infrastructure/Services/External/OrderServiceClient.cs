using Finance_Service.src._02_Application.Interfaces;
using Polly;
using Polly.Retry;

namespace Finance_Service.src._03_Infrastructure.Services.External
{
    public class OrderServiceClient : IOrderServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private readonly ILogger<OrderServiceClient> _logger;

        public OrderServiceClient(HttpClient httpClient, ILogger<OrderServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning("Retry {Count} for Order Service after {Delay}s due to: {Error}",
                            retryCount, timeSpan.TotalSeconds, outcome.Exception?.Message);
                    });
        }

        public async Task<bool> ConfirmOrderAsync(Guid orderId)
        {
            // فرض بر این است که اندپوینت تایید سفارش در OrderService این شکلی است
            // یا نیاز به بدن نیست و فقط اطلاع می‌دهیم
            var response = await _retryPolicy.ExecuteAsync(() =>
                _httpClient.PostAsJsonAsync($"/api/orders/{orderId}/confirm", new { }));

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Order Service returned {StatusCode} for OrderId {OrderId}",
                    response.StatusCode, orderId);
                return false;
            }

            return true;
        }
    }
}
