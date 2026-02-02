using Order_Service.src._01_Domain.Core.ValueObjects;

namespace Order_Service.src._02_Application.Interfaces
{
    public interface IPaymentGateway
    {
        Task<PaymentGatewayResult> ProcessPaymentAsync(Money amount, string cardNumber, string cvv2, string expMonth, string expYear, string pin);
        Task<RefundGatewayResult> ProcessRefundAsync(Guid transactionId, Money amount);
    }

    public class PaymentGatewayResult
    {
        public bool IsSuccessful { get; set; }
        public string? TransactionId { get; set; }
        public string? Message { get; set; }
        public string? PaymentUrl { get; set; }
    }

    public class RefundGatewayResult
    {
        public bool IsSuccessful { get; set; }
        public string? RefundId { get; set; }
        public string? Message { get; set; }
    }
}
