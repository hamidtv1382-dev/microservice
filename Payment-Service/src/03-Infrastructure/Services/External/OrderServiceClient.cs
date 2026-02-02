using Payment_Service.src._02_Application.Interfaces;
using System.Net.Http.Headers;
using System.Security.Claims;
using Polly;
using Polly.Retry;

namespace Payment_Service.src._03_Infrastructure.Services.External
{
    public class OrderServiceClient : IOrderServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private readonly ILogger<OrderServiceClient> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderServiceClient(HttpClient httpClient, ILogger<OrderServiceClient> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<OrderInfoDto?> GetOrderInfoAsync(Guid orderId)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && user.Identity?.IsAuthenticated == true)
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
                }
            }

            var response = await _retryPolicy.ExecuteAsync(() =>
                _httpClient.GetAsync($"/api/orders/{orderId}"));

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Order Service returned {StatusCode} for OrderId {OrderId}",
                    response.StatusCode, orderId);
                return null;
            }

            var orderResponse = await response.Content.ReadFromJsonAsync<OrderServiceResponseDto>();

            if (orderResponse == null) return null;

            Guid buyerId = Guid.Empty;
            if (user != null)
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.FindFirst("sub")?.Value;
                if (Guid.TryParse(userIdClaim, out var parsedId))
                {
                    buyerId = parsedId;
                }
            }

            return new OrderInfoDto
            {
                OrderId = orderResponse.Id,
                BuyerId = buyerId,
                FinalAmount = orderResponse.FinalAmount
            };
        }

        // کلاس داخلی برای دیسریالایز کردن JSON پاسخ
        private class OrderServiceResponseDto
        {
            public Guid Id { get; set; }
            public decimal FinalAmount { get; set; }
        }
    }
}