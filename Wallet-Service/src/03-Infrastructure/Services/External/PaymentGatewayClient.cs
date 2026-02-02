using Wallet_Service.src._01_Domain.Core.ValueObjects;
using Wallet_Service.src._02_Application.Interfaces;

namespace Wallet_Service.src._03_Infrastructure.Services.External
{
    public class PaymentGatewayClient : IExternalPaymentGateway
    {
        public async Task<GatewayResult> ProcessTopUpAsync(Money amount, string method)
        {
            // Simulate external API call to payment gateway for wallet top-up
            await Task.Delay(100);

            var random = new Random();
            var isSuccess = random.Next(1, 10) > 1; // 90% success rate

            if (isSuccess)
            {
                return new GatewayResult
                {
                    Success = true,
                    TransactionId = $"GATEWAY-{Guid.NewGuid()}",
                    Message = "Top-up successful."
                };
            }

            return new GatewayResult
            {
                Success = false,
                TransactionId = null,
                Message = "Payment declined by gateway."
            };
        }
    }
}
