using Payment_Service.src._01_Domain.Core.Entities;
using Payment_Service.src._02_Application.Interfaces;

namespace Payment_Service.src._03_Infrastructure.Services.External
{
    public class PaymentGatewayClient : IPaymentGateway
    {
        public async Task<GatewayResult> ProcessPaymentAsync(Payment payment)
        {
            // Simulate external API call
            await Task.Delay(100);

            // Mock Logic: Randomly fail for demonstration purposes
            var random = new Random();
            var isSuccess = random.Next(1, 10) > 2;

            if (isSuccess)
            {
                return new GatewayResult
                {
                    Success = true,
                    TransactionId = $"EXT-{Guid.NewGuid()}",
                    Message = "Payment processed successfully."
                };
            }

            return new GatewayResult
            {
                Success = false,
                TransactionId = null,
                Message = "Payment declined by gateway."
            };
        }

        public async Task<GatewayVerificationResult> VerifyPaymentAsync(string transactionNumber)
        {
            await Task.Delay(50);
            return new GatewayVerificationResult
            {
                IsVerified = true,
                TransactionId = transactionNumber
            };
        }

        public async Task<GatewayResult> ProcessRefundAsync(Refund refund)
        {
            await Task.Delay(150);

            var isSuccess = true;

            if (isSuccess)
            {
                return new GatewayResult
                {
                    Success = true,
                    TransactionId = $"EXT-REF-{Guid.NewGuid()}",
                    Message = "Refund processed successfully."
                };
            }

            return new GatewayResult
            {
                Success = false,
                TransactionId = null,
                Message = "Refund failed at gateway."
            };
        }
    }
}
