using Payment_Service.src._02_Application.Interfaces;
using Polly;
using Polly.Retry;

namespace Payment_Service.src._03_Infrastructure.Services.External
{
    public class WalletServiceClient : IWalletServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private readonly ILogger<WalletServiceClient> _logger;

        public WalletServiceClient(HttpClient httpClient, ILogger<WalletServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning("Retry {Count} for Wallet Service after {Delay}s due to: {Error}",
                            retryCount, timeSpan.TotalSeconds, outcome.Exception?.Message);
                    });
        }

        public async Task<bool> DeductFundsAsync(Guid userId, decimal amount)
        {
            var payload = new { Amount = amount };
            var response = await _retryPolicy.ExecuteAsync(() =>
                _httpClient.PostAsJsonAsync($"/api/wallets/{userId}/deduct", payload));

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Wallet Service returned {StatusCode} for UserId {UserId}", response.StatusCode, userId);
                return false;
            }

            return true;
        }

        public async Task<bool> RefundFundsAsync(Guid userId, decimal amount)
        {
            var payload = new { Amount = amount };
            var response = await _retryPolicy.ExecuteAsync(() =>
                _httpClient.PostAsJsonAsync($"/api/wallets/{userId}/refund", payload));

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Wallet Service returned {StatusCode} for UserId {UserId}", response.StatusCode, userId);
                return false;
            }

            return true;
        }
    }
}