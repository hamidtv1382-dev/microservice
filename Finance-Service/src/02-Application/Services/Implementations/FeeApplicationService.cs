using Finance_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Finance_Service.src._01_Domain.Services.Interfaces;
using Finance_Service.src._02_Application.DTOs.Requests;
using Finance_Service.src._02_Application.DTOs.Responses;
using Finance_Service.src._02_Application.Exceptions;
using Finance_Service.src._02_Application.Mappings;
using Finance_Service.src._02_Application.Services.Interfaces;

namespace Finance_Service.src._02_Application.Services.Implementations
{
    public class FeeApplicationService : IFeeApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFinanceDomainService _financeDomainService;
        private readonly FinanceMappingProfile _mapper;

        public FeeApplicationService(IUnitOfWork unitOfWork, IFinanceDomainService financeDomainService, FinanceMappingProfile mapper)
        {
            _unitOfWork = unitOfWork;
            _financeDomainService = financeDomainService;
            _mapper = mapper;
        }

        public async Task<FeeResponseDto> ApplyFeeAsync(ApplyFeeRequestDto request)
        {
            var fee = await _financeDomainService.CalculateAndApplyFeeAsync(request.OrderId, request.OrderAmount, request.SellerId);
            await _unitOfWork.Fees.AddAsync(fee);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.MapToFeeResponseDto(fee);
        }

        public async Task<FeeResponseDto> GetFeeByIdAsync(Guid id)
        {
            var fee = await _unitOfWork.Fees.GetByIdAsync(id);
            if (fee == null) throw new FeeApplicationFailedException("Fee not found.");
            return _mapper.MapToFeeResponseDto(fee);
        }

        public async Task<IEnumerable<FeeResponseDto>> GetFeesByOrderIdAsync(Guid orderId)
        {
            var fees = await _unitOfWork.Fees.GetByOrderIdAsync(orderId);
            return _mapper.MapToFeeResponseDtoList(fees);
        }
    }
}
