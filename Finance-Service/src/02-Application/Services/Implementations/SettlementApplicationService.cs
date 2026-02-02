using Finance_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Finance_Service.src._01_Domain.Services.Interfaces;
using Finance_Service.src._02_Application.DTOs.Requests;
using Finance_Service.src._02_Application.DTOs.Responses;
using Finance_Service.src._02_Application.Exceptions;
using Finance_Service.src._02_Application.Interfaces;
using Finance_Service.src._02_Application.Mappings;
using Finance_Service.src._02_Application.Services.Interfaces;

namespace Finance_Service.src._02_Application.Services.Implementations
{
    public class SettlementApplicationService : ISettlementApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFinanceDomainService _financeDomainService;
        private readonly FinanceMappingProfile _mapper;
        private readonly IExternalPaymentService _externalPaymentService; // For bank transfers

        public SettlementApplicationService(IUnitOfWork unitOfWork, IFinanceDomainService financeDomainService, FinanceMappingProfile mapper, IExternalPaymentService externalPaymentService)
        {
            _unitOfWork = unitOfWork;
            _financeDomainService = financeDomainService;
            _mapper = mapper;
            _externalPaymentService = externalPaymentService;
        }

        public async Task<SettlementResponseDto> CreateSettlementAsync(CreateSettlementRequestDto request)
        {
            // --- اصلاح شده: ارسال پارامترهای جدید به متد Domain Service ---
            var settlement = await _financeDomainService.CreateSellerSettlementAsync(
                request.SellerId,
                request.TotalAmount,
                request.BankAccountInfo ?? string.Empty, // در صورت null بودن، رشته خالی ارسال کن
                request.DueDate
            );
            // ---------------------------------------------------------------

            await _unitOfWork.Settlements.AddAsync(settlement);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.MapToSettlementResponseDto(settlement);
        }

        public async Task<SettlementResponseDto> ProcessSettlementAsync(Guid settlementId)
        {
            var settlement = await _unitOfWork.Settlements.GetByIdAsync(settlementId);
            if (settlement == null) throw new SettlementFailedException("Settlement not found.");

            settlement.StartProcessing();
            await _unitOfWork.SaveChangesAsync();

            var success = await _externalPaymentService.TransferToBankAccountAsync(settlement.TotalAmount, settlement.BankAccountInfo);

            if (success)
            {
                settlement.CompleteSettlement(Guid.NewGuid().ToString()); // Mock Reference
            }
            else
            {
                settlement.FailSettlement("Bank transfer failed");
            }

            await _unitOfWork.SaveChangesAsync();
            return _mapper.MapToSettlementResponseDto(settlement);
        }

        public async Task<IEnumerable<SettlementResponseDto>> GetSettlementsBySellerIdAsync(Guid sellerId)
        {
            var settlements = await _unitOfWork.Settlements.GetBySellerIdAsync(sellerId);
            return _mapper.MapToSettlementResponseDtoList(settlements);
        }
    }
}
