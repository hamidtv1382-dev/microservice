using Seller_Finance_Service.src._02_Application.Interfaces;
using System.Text;
using System.Text.Json;

namespace Seller_Finance_Service.src._03_Infrastructure.Services.External
{
    public class WalletServiceClient : IWalletService
    {
        private readonly HttpClient _httpClient;

        public WalletServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddFundsAsync(Guid walletId, decimal amount)
        {
            // Call Wallet Service to add funds
            var payload = new { WalletId = walletId, Amount = amount };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://wallet-service/api/wallets/add-funds", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeductFundsAsync(Guid walletId, decimal amount)
        {
            // Call Wallet Service to deduct funds
            var payload = new { WalletId = walletId, Amount = amount };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://wallet-service/api/wallets/deduct-funds", content);
            return response.IsSuccessStatusCode;
        }
    }
}
