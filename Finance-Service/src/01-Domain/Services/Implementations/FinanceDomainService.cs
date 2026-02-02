using Finance_Service.src._01_Domain.Core.Entities;
using Finance_Service.src._01_Domain.Core.Enums;
using Finance_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Finance_Service.src._01_Domain.Core.ValueObjects;
using Finance_Service.src._01_Domain.Services.Interfaces;

namespace Finance_Service.src._01_Domain.Services.Implementations
{
    public class FinanceDomainService : IFinanceDomainService
    {
        private readonly IUnitOfWork _unitOfWork;

        // تزریق وابستگی برای دسترسی به دیتابیس (برای محاسبه دقیق در آینده یا اگر لازم بود)
        public FinanceDomainService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Fee> CalculateAndApplyFeeAsync(Guid orderId, Money totalOrderAmount, Guid? sellerId)
        {
            // Logic: Platform fee is 2% of total order amount
            decimal feeAmount = totalOrderAmount.Amount * 0.02m;
            var fee = new Fee(orderId, new Money(feeAmount, totalOrderAmount.Currency), FeeType.PlatformFee, "Platform Service Fee", sellerId, "System");
            return await Task.FromResult(fee);
        }

        public async Task<Commission> CalculateCommissionAsync(Guid orderId, Guid sellerId, Money saleAmount)
        {
            // Logic: Sales commission is 5% of sale amount
            decimal commissionAmount = saleAmount.Amount * 0.05m;
            var commission = new Commission(orderId, sellerId, new Money(commissionAmount, saleAmount.Currency), CommissionType.SalesCommission, 5.0m, "System");
            return await Task.FromResult(commission);
        }

        // --- متد بروزرسانی شده ---
        public async Task<Settlement> CreateSellerSettlementAsync(Guid sellerId, Money totalAmount, string bankInfo, DateTime dueDate)
        {
            // Logic: مبلغ تسویه را دریافت می‌کند (که توسط سرویس SellerFinance ارسال شده است)
            // نیازی به محاسبه مجدد نیست، چون SellerFinance موجودی را از کیف پول فروشنده کم کرده و مبلغ دقیق را اعلام کرده است.

            var settlement = new Settlement(sellerId, totalAmount, dueDate, "SellerFinanceService");

            // اگر موجودیت Settlement فیلدی برای ذخیره اطلاعات بانک دارد، اینجا ست می‌شود:
            // در کلاس Settlement شما فیلد BankAccountInfo (string) وجود دارد.

            return await Task.FromResult(settlement);
        }

        public Task<bool> ProcessSettlementAsync(Guid settlementId)
        {
            // Logic to connect to banking API would go here
            return Task.FromResult(true);
        }
    }
}
