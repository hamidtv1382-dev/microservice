using Order_Service.src._02_Application.DTOs.Requests;
using Order_Service.src._02_Application.DTOs.Responses;

namespace Order_Service.src._02_Application.Services.Interfaces
{
    public interface IAdminDiscountApplicationService
    {
        Task<DiscountDetailResponseDto> CreateDiscountAsync(CreateDiscountRequestDto request);
        Task<DiscountDetailResponseDto?> GetDiscountByIdAsync(Guid id);
        Task<IEnumerable<DiscountSummaryResponseDto>> GetAllDiscountsAsync();
        Task<DiscountDetailResponseDto?> UpdateDiscountAsync(Guid id, UpdateDiscountRequestDto request);
        Task DeleteDiscountAsync(Guid id);
    }
}
