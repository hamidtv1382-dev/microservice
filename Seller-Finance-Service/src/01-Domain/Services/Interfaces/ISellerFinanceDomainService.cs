using Seller_Finance_Service.src._01_Domain.Core.Entities;
using Seller_Finance_Service.src._01_Domain.Core.Enums;
using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Seller_Finance_Service.src._01_Domain.Services.Interfaces
{
    public interface ISellerFinanceDomainService
    {
        Task<SellerAccount> CreateAccountAsync(Guid sellerId, BankAccountInfo bankAccount);
        Task<SellerAccount?> GetAccountAsync(Guid sellerId);
        Task<bool> RecordEarningAsync(Guid sellerId, Guid orderId, Money amount);
        Task<bool> ReleaseBalanceAsync(Guid sellerId, Guid orderId);
        Task<bool> HoldBalanceAsync(Guid sellerId, Money amount, HoldReasonType reason, string description);
    }
}
