using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Seller_Finance_Service.src._02_Application.DTOs.Responses
{
    public class SellerAccountResponseDto
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; set; }
        public bool IsActive { get; set; }
        public Money AvailableBalance { get; set; }
        public Money PendingBalance { get; set; }
        public Money HoldBalance { get; set; }
        public BankAccountInfo BankAccount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
