using Wallet_Service.src._02_Application.DTOs.Requests;
using Wallet_Service.src._02_Application.DTOs.Responses;

namespace Wallet_Service.src._02_Application.Services.Interfaces
{
    public interface IWalletApplicationService
    {
        Task<WalletBalanceResponseDto> GetBalanceAsync(Guid ownerId);
        Task<WalletTransactionResponseDto> AddFundsAsync(AddFundsRequestDto request);
        Task<WalletTransactionResponseDto> DeductFundsAsync(DeductFundsRequestDto request);
        Task<WalletTransactionResponseDto> DeductFundsByUserIdAsync(DeductFundsByUserIdRequestDto request);
    }
}
