using Finance_Service.src._01_Domain.Core.Entities;
using Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Finance_Service.src._01_Domain.Services.Interfaces
{
    public interface IFinanceDomainService
    {
        Task<Fee> CalculateAndApplyFeeAsync(Guid orderId, Money totalOrderAmount, Guid? sellerId);
        Task<Commission> CalculateCommissionAsync(Guid orderId, Guid sellerId, Money saleAmount);

        // متد بروزرسانی شده: پارامترهای مبلغ و اطلاعات بانک اضافه شدند
        Task<Settlement> CreateSellerSettlementAsync(Guid sellerId, Money totalAmount, string bankInfo, DateTime dueDate);

        Task<bool> ProcessSettlementAsync(Guid settlementId);
    }
}
