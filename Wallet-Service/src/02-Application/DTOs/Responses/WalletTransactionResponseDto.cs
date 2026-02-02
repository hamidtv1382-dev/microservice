using Wallet_Service.src._01_Domain.Core.Enums;
using Wallet_Service.src._01_Domain.Core.ValueObjects;

namespace Wallet_Service.src._02_Application.DTOs.Responses
{
    public class WalletTransactionResponseDto
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public TransactionType Type { get; set; }
        public Money Amount { get; set; }
        public string ReferenceId { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
