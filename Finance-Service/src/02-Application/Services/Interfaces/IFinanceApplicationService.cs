using Finance_Service.src._02_Application.DTOs.Requests;
using Finance_Service.src._02_Application.DTOs.Responses;

namespace Finance_Service.src._02_Application.Services.Interfaces
{
    public interface IFinanceApplicationService
    {
        Task<FeeResponseDto> ApplyFeeAsync(ApplyFeeRequestDto request);
        Task<FeeResponseDto> GetFeeByIdAsync(Guid id);
        Task<IEnumerable<FeeResponseDto>> GetFeesByOrderIdAsync(Guid orderId);

        Task<CommissionResponseDto> ProcessCommissionAsync(ProcessCommissionRequestDto request);
        Task<CommissionResponseDto> GetCommissionByIdAsync(Guid id);
        Task<IEnumerable<CommissionResponseDto>> GetCommissionsBySellerIdAsync(Guid sellerId);

        Task<SettlementResponseDto> CreateSettlementAsync(CreateSettlementRequestDto request);
        Task<SettlementResponseDto> ProcessSettlementAsync(Guid settlementId);
        Task<IEnumerable<SettlementResponseDto>> GetSettlementsBySellerIdAsync(Guid sellerId);
    }
}
