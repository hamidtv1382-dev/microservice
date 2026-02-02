using Seller_Finance_Service.src._01_Domain.Core.Enums;
using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Seller_Finance_Service.src._02_Application.DTOs.Responses
{
    public class SellerTransactionResponseDto
    {
        public Guid Id { get; set; }
        public Money Amount { get; set; }
        public SellerTransactionType Type { get; set; }
        public SellerTransactionStatus Status { get; set; }
        public string ReferenceId { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
