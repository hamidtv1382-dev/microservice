using Seller_Finance_Service.src._02_Application.DTOs.Requests;
using Seller_Finance_Service.src._02_Application.DTOs.Responses;

namespace Seller_Finance_Service.src._02_Application.Services.Interfaces
{
    public interface ISellerAccountApplicationService
    {
        Task<SellerAccountResponseDto> CreateAccountAsync(CreateSellerAccountRequestDto request);
        Task<SellerAccountResponseDto?> GetAccountBySellerIdAsync(Guid sellerId);
        Task<SellerAccountResponseDto> UpdateBankAccountAsync(UpdateSellerBankAccountRequestDto request);
        Task ActivateAccountAsync(Guid sellerId);
        Task DeactivateAccountAsync(Guid sellerId);
    }
}
