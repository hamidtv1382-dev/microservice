using Payment_Service.src._01_Domain.Core.Enums;
using Payment_Service.src._01_Domain.Core.ValueObjects;

namespace Payment_Service.src._02_Application.DTOs.Responses
{
    public class RefundResponseDto
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public Money Amount { get; set; }
        public RefundStatus Status { get; set; }
        public string Reason { get; set; }
        public string? ExternalRefundId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RefundedAt { get; set; }
    }
}
