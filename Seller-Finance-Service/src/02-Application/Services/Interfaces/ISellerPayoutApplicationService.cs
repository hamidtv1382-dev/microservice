using Seller_Finance_Service.src._02_Application.DTOs.Requests;
using Seller_Finance_Service.src._02_Application.DTOs.Responses;

namespace Seller_Finance_Service.src._02_Application.Services.Interfaces
{
    public interface ISellerPayoutApplicationService
    {
        Task<SellerPayoutResponseDto> RequestPayoutAsync(RequestSellerPayoutDto request);
        Task<SellerPayoutResponseDto> GetPayoutStatusAsync(Guid payoutId);
    }
}
