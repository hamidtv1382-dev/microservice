using Finance_Service.src._02_Application.DTOs.Requests;
using Finance_Service.src._02_Application.DTOs.Responses;

namespace Finance_Service.src._02_Application.Services.Interfaces
{
    public interface ISettlementApplicationService
    {
        Task<SettlementResponseDto> CreateSettlementAsync(CreateSettlementRequestDto request);
        Task<SettlementResponseDto> ProcessSettlementAsync(Guid settlementId);
        Task<IEnumerable<SettlementResponseDto>> GetSettlementsBySellerIdAsync(Guid sellerId);
    }
}
