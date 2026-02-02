using Payment_Service.src._01_Domain.Core.Entities;

namespace Payment_Service.src._02_Application.Interfaces
{
    public interface IPaymentGateway
    {
        Task<GatewayResult> ProcessPaymentAsync(Payment payment);
        Task<GatewayVerificationResult> VerifyPaymentAsync(string transactionNumber);
        Task<GatewayResult> ProcessRefundAsync(Refund refund);
    }

    public class GatewayResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }
    }

    public class GatewayVerificationResult
    {
        public bool IsVerified { get; set; }
        public string TransactionId { get; set; }
    }

    // اینترفیس ارتباط با سرویس کیف پول
    public interface IWalletServiceClient
    {
        Task<bool> DeductFundsAsync(Guid userId, decimal amount);
        Task<bool> RefundFundsAsync(Guid userId, decimal amount);
    }

    // اینترفیس ارتباط با سرویس سفارش
    public interface IOrderServiceClient
    {
        Task<OrderInfoDto?> GetOrderInfoAsync(Guid orderId);
    }

    // مدل مشترک برای اطلاعات سفارش
    public class OrderInfoDto
    {
        public Guid OrderId { get; set; }
        public Guid BuyerId { get; set; }
        public decimal FinalAmount { get; set; }
    }
}