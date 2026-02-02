using Polly;
using Polly.Retry;

namespace Order_Service.src._03_Infrastructure.Services.External
{
    public class SellerFinanceServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

        public SellerFinanceServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public async Task<bool> RecordSellerEarningAsync(Guid sellerId, Guid orderId, decimal amount, Guid transactionId)
        {
            var payload = new { SellerId = sellerId, OrderId = orderId, Amount = amount, TransactionId = transactionId };
            var response = await _retryPolicy.ExecuteAsync(() =>
                _httpClient.PostAsJsonAsync("/api/sellerbalances/earning", payload));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ReleaseSellerBalanceAsync(Guid orderId, Guid transactionId)
        {
            var payload = new { OrderId = orderId, TransactionId = transactionId };
            var response = await _retryPolicy.ExecuteAsync(() =>
                _httpClient.PostAsJsonAsync("/api/sellerbalances/balance/release", payload));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RequestPayoutAsync(Guid sellerId)
        {
            var response = await _retryPolicy.ExecuteAsync(() =>
                _httpClient.PostAsJsonAsync("/api/sellerpayouts", new { SellerId = sellerId }));

            return response.IsSuccessStatusCode;
        }
    }
}