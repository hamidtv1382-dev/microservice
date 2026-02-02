using Wallet_Service.src._01_Domain.Core.ValueObjects;

namespace Wallet_Service.src._02_Application.Interfaces
{
    public interface IExternalPaymentGateway
    {
        Task<GatewayResult> ProcessTopUpAsync(Money amount, string method);
    }

    public class GatewayResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }
    }
}
