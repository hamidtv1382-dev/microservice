using Finance_Service.src._02_Application.DTOs.Requests;
using Finance_Service.src._02_Application.DTOs.Responses;

namespace Finance_Service.src._02_Application.Services.Interfaces
{
    public interface IFeeApplicationService
    {
        Task<FeeResponseDto> ApplyFeeAsync(ApplyFeeRequestDto request);
        Task<FeeResponseDto> GetFeeByIdAsync(Guid id);
        Task<IEnumerable<FeeResponseDto>> GetFeesByOrderIdAsync(Guid orderId);
    }
}
