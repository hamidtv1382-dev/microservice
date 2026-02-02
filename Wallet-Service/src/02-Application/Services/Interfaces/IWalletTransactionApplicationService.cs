using Wallet_Service.src._02_Application.DTOs.Requests;
using Wallet_Service.src._02_Application.DTOs.Responses;

namespace Wallet_Service.src._02_Application.Services.Interfaces
{
    public interface IWalletTransactionApplicationService
    {
        Task<IEnumerable<WalletTransactionResponseDto>> GetTransactionsAsync(GetWalletTransactionsRequestDto request);
    }
}
