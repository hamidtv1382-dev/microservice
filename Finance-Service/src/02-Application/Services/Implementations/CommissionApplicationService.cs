using Finance_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Finance_Service.src._01_Domain.Services.Interfaces;
using Finance_Service.src._02_Application.DTOs.Requests;
using Finance_Service.src._02_Application.DTOs.Responses;
using Finance_Service.src._02_Application.Exceptions;
using Finance_Service.src._02_Application.Mappings;
using Finance_Service.src._02_Application.Services.Interfaces;

namespace Finance_Service.src._02_Application.Services.Implementations
{
    public class CommissionApplicationService : ICommissionApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFinanceDomainService _financeDomainService;
        private readonly FinanceMappingProfile _mapper;

        public CommissionApplicationService(IUnitOfWork unitOfWork, IFinanceDomainService financeDomainService, FinanceMappingProfile mapper)
        {
            _unitOfWork = unitOfWork;
            _financeDomainService = financeDomainService;
            _mapper = mapper;
        }

        public async Task<CommissionResponseDto> ProcessCommissionAsync(ProcessCommissionRequestDto request)
        {
            var commission = await _financeDomainService.CalculateCommissionAsync(request.OrderId, request.SellerId, request.SaleAmount);
            await _unitOfWork.Commissions.AddAsync(commission);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.MapToCommissionResponseDto(commission);
        }

        public async Task<CommissionResponseDto> GetCommissionByIdAsync(Guid id)
        {
            var commission = await _unitOfWork.Commissions.GetByIdAsync(id);
            if (commission == null) throw new CommissionProcessingFailedException("Commission not found.");
            return _mapper.MapToCommissionResponseDto(commission);
        }

        public async Task<IEnumerable<CommissionResponseDto>> GetCommissionsBySellerIdAsync(Guid sellerId)
        {
            var commissions = await _unitOfWork.Commissions.GetBySellerIdAsync(sellerId);
            return _mapper.MapToCommissionResponseDtoList(commissions);
        }
    }
}
