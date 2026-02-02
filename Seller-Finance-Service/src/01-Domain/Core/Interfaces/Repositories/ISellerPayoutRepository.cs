using Seller_Finance_Service.src._01_Domain.Core.Entities;

namespace Seller_Finance_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface ISellerPayoutRepository
    {
        Task<SellerPayout?> GetByIdAsync(Guid id);
        Task<IEnumerable<SellerPayout>> GetBySellerAccountIdAsync(Guid accountId);
        Task<IEnumerable<SellerPayout>> GetRequestedPayoutsAsync();
        Task AddAsync(SellerPayout payout);
        Task UpdateAsync(SellerPayout payout);
        Task DeleteAsync(SellerPayout payout);
    }
}
