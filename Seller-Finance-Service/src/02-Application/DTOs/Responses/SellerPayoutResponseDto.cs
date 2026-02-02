using Seller_Finance_Service.src._01_Domain.Core.Enums;
using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Seller_Finance_Service.src._02_Application.DTOs.Responses
{
    public class SellerPayoutResponseDto
    {
        public Guid Id { get; set; }
        public Money Amount { get; set; }
        public PayoutStatus Status { get; set; }
        public DateTime? RequestedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? FailureReason { get; set; }
    }
}
