using Seller_Finance_Service.src._02_Application.DTOs.Requests;
using Seller_Finance_Service.src._02_Application.DTOs.Responses;

namespace Seller_Finance_Service.src._02_Application.Services.Interfaces
{
    public interface ISellerBalanceApplicationService
    {
        Task<SellerBalanceResponseDto> GetBalanceAsync(Guid sellerId);
        Task<SellerTransactionResponseDto> RecordEarningAsync(RecordSellerEarningRequestDto request);
        Task<SellerTransactionResponseDto> ReleaseBalanceAsync(ReleaseSellerBalanceRequestDto request);
        Task<bool> HoldBalanceAsync(Guid sellerId, decimal amount, string reason, string description);
    }
}
