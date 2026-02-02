using Seller_Finance_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;
using Seller_Finance_Service.src._01_Domain.Services.Interfaces;
using Seller_Finance_Service.src._02_Application.DTOs.Requests;
using Seller_Finance_Service.src._02_Application.DTOs.Responses;
using Seller_Finance_Service.src._02_Application.Exceptions;
using Seller_Finance_Service.src._02_Application.Interfaces;
using Seller_Finance_Service.src._02_Application.Mappings;
using Seller_Finance_Service.src._02_Application.Services.Interfaces;

namespace Seller_Finance_Service.src._02_Application.Services.Implementations
{
    public class SellerPayoutApplicationService : ISellerPayoutApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPayoutPolicyService _payoutPolicy;
        private readonly IFinanceService _financeService; // To trigger transfer
        private readonly SellerFinanceMappingProfile _mapper;

        public SellerPayoutApplicationService(IUnitOfWork unitOfWork, IPayoutPolicyService payoutPolicy, IFinanceService financeService, SellerFinanceMappingProfile mapper)
        {
            _unitOfWork = unitOfWork;
            _payoutPolicy = payoutPolicy;
            _financeService = financeService;
            _mapper = mapper;
        }

        public async Task<SellerPayoutResponseDto> RequestPayoutAsync(RequestSellerPayoutDto request)
        {
            var account = await _unitOfWork.SellerAccounts.GetBySellerIdAsync(request.SellerId);
            if (account == null) throw new SellerAccountNotFoundException("Seller account not found.");

            var payout = _payoutPolicy.CreatePayout(account, new Money(request.Amount, "IRR"));

            // 1. کم کردن موجودی از کیف پول فروشنده (در سیستم خودمان)
            account.Balance.DeductFromAvailable(payout.Amount);

            // 2. تغییر وضعیت به در حال پردازش
            payout.MarkAsProcessing();

            await _unitOfWork.SellerPayouts.AddAsync(payout);
            _unitOfWork.SellerAccounts.UpdateAsync(account);
            await _unitOfWork.SaveChangesAsync();

            // 3. ارسال درخواست به سرویس مالی برای ثبت سند و انجام انتقال بانکی
            // نکته: اینجا SellerId را هم پاس می‌دهیم
            var settlementCreated = await _financeService.RequestSettlementCreationAsync(
                account.SellerId,
                account.BankAccount,
                payout.Amount
            );

            if (settlementCreated)
            {
                payout.MarkAsCompleted("SETTLEMENT-" + Guid.NewGuid());
            }
            else
            {
                payout.MarkAsFailed("Finance service integration failed");
                // بازگشت پول به کیف پول در صورت شکست
                account.Balance.ReleaseFromPendingToAvailable(payout.Amount);
            }

            await _unitOfWork.SaveChangesAsync();
            return _mapper.MapToPayoutResponseDto(payout);
        }

        public async Task<SellerPayoutResponseDto> GetPayoutStatusAsync(Guid payoutId)
        {
            var payout = await _unitOfWork.SellerPayouts.GetByIdAsync(payoutId);
            if (payout == null) throw new SellerPayoutFailedException("Payout not found.");

            return _mapper.MapToPayoutResponseDto(payout);
        }
    }
}
