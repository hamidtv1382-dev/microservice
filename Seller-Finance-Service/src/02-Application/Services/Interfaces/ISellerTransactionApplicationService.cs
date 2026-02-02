using Seller_Finance_Service.src._02_Application.DTOs.Requests;
using Seller_Finance_Service.src._02_Application.DTOs.Responses;

namespace Seller_Finance_Service.src._02_Application.Services.Interfaces
{
    public interface ISellerTransactionApplicationService
    {
        Task<IEnumerable<SellerTransactionResponseDto>> GetTransactionsAsync(GetSellerTransactionsRequestDto request);
    }
}
