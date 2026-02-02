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
    public class FinanceApplicationService : IFinanceApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFinanceDomainService _financeDomainService;
        private readonly IOrderServiceClient _orderServiceClient;
        private readonly FinanceMappingProfile _mapper;
        private readonly ILogger<FinanceApplicationService> _logger;

        public FinanceApplicationService(IUnitOfWork unitOfWork, IFinanceDomainService financeDomainService, IOrderServiceClient orderServiceClient, FinanceMappingProfile mapper, ILogger<FinanceApplicationService> logger)
        {
            _unitOfWork = unitOfWork;
            _financeDomainService = financeDomainService;
            _orderServiceClient = orderServiceClient;
            _mapper = mapper;
            _logger = logger;
        }

        // Fees
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

        // Commissions
        public async Task<CommissionResponseDto> ProcessCommissionAsync(ProcessCommissionRequestDto request)
        {
            var commission = await _financeDomainService.CalculateCommissionAsync(request.OrderId, request.SellerId, request.SaleAmount);

            // ثبت کمیسیون در دیتابیس
            await _unitOfWork.Commissions.AddAsync(commission);
            await _unitOfWork.SaveChangesAsync();

            // --- ارتباط اختیاری: تایید سفارش در سرویس Order ---
            try
            {
                var orderConfirmed = await _orderServiceClient.ConfirmOrderAsync(request.OrderId);
                if (!orderConfirmed)
                {
                    _logger.LogWarning($"Failed to confirm order {request.OrderId} in OrderService.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling OrderService to confirm {request.OrderId}");
            }
            // ---------------------------------------------------------------

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

        // Settlements
        public async Task<SettlementResponseDto> CreateSettlementAsync(CreateSettlementRequestDto request)
        {
            var settlement = await _financeDomainService.CreateSellerSettlementAsync(
                request.SellerId,
                request.TotalAmount,
                request.BankAccountInfo ?? string.Empty,
                request.DueDate
            );

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

            var success = await _financeDomainService.ProcessSettlementAsync(settlementId);

            if (success)
            {
                settlement.CompleteSettlement(Guid.NewGuid().ToString());
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
