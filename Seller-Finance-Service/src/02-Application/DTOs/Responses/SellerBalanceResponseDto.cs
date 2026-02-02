using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Seller_Finance_Service.src._02_Application.DTOs.Responses
{
    public class SellerBalanceResponseDto
    {
        public Guid SellerId { get; set; }
        public Money AvailableBalance { get; set; }
        public Money PendingBalance { get; set; }
        public Money HoldBalance { get; set; }
    }
}
