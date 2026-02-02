using Wallet_Service.src._01_Domain.Core.ValueObjects;

namespace Wallet_Service.src._02_Application.DTOs.Responses
{
    public class WalletBalanceResponseDto
    {
        public Guid WalletId { get; set; }
        public Guid OwnerId { get; set; }
        public Money Balance { get; set; }
        public bool IsActive { get; set; }
    }
}
