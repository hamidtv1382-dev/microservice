using Finance_Service.src._02_Application.DTOs.Requests;
using Finance_Service.src._02_Application.DTOs.Responses;

namespace Finance_Service.src._02_Application.Services.Interfaces
{
    public interface ICommissionApplicationService
    {
        Task<CommissionResponseDto> ProcessCommissionAsync(ProcessCommissionRequestDto request);
        Task<CommissionResponseDto> GetCommissionByIdAsync(Guid id);
        Task<IEnumerable<CommissionResponseDto>> GetCommissionsBySellerIdAsync(Guid sellerId);
    }
}
