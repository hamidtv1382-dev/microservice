using Wallet_Service.src._01_Domain.Core.Entities;
using Wallet_Service.src._02_Application.DTOs.Responses;

namespace Wallet_Service.src._02_Application.Mappings
{
    public class WalletMappingProfile
    {
        public WalletBalanceResponseDto MapToWalletBalanceResponseDto(Wallet wallet)
        {
            return new WalletBalanceResponseDto
            {
                WalletId = wallet.Id,
                OwnerId = wallet.OwnerId,
                Balance = wallet.Balance,
                IsActive = wallet.IsActive
            };
        }

        public WalletTransactionResponseDto MapToWalletTransactionResponseDto(WalletTransaction transaction)
        {
            return new WalletTransactionResponseDto
            {
                Id = transaction.Id,
                WalletId = transaction.WalletId,
                Type = transaction.Type,
                Amount = transaction.Amount,
                ReferenceId = transaction.ReferenceId,
                Description = transaction.Description,
                TransactionDate = transaction.TransactionDate
            };
        }
    }
}
