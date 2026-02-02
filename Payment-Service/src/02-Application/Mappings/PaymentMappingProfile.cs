using Payment_Service.src._01_Domain.Core.Entities;
using Payment_Service.src._02_Application.DTOs.Responses;

namespace Payment_Service.src._02_Application.Mappings
{
    public class PaymentMappingProfile
    {
        public PaymentResponseDto MapToPaymentResponseDto(Payment payment)
        {
            return new PaymentResponseDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                Status = payment.Status,
                Method = payment.Method,
                TransactionNumber = payment.TransactionNumber.Value,
                ExternalTransactionId = payment.ExternalTransactionId,
                CreatedAt = payment.CreatedAt,
                PaidAt = payment.PaidAt
            };
        }

        public RefundResponseDto MapToRefundResponseDto(Refund refund)
        {
            return new RefundResponseDto
            {
                Id = refund.Id,
                PaymentId = refund.PaymentId,
                Amount = refund.Amount,
                Status = refund.Status,
                Reason = refund.Reason,
                ExternalRefundId = refund.ExternalRefundId,
                CreatedAt = refund.CreatedAt,
                RefundedAt = refund.RefundedAt
            };
        }
    }
}
