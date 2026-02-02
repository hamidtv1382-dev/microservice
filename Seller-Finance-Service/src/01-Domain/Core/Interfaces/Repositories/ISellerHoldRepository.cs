using Seller_Finance_Service.src._01_Domain.Core.Entities;

namespace Seller_Finance_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface ISellerHoldRepository
    {
        Task<SellerHold?> GetByIdAsync(Guid id);
        Task<IEnumerable<SellerHold>> GetActiveHoldsBySellerAccountIdAsync(Guid accountId);
        Task AddAsync(SellerHold hold);
        Task UpdateAsync(SellerHold hold);
        Task DeleteAsync(SellerHold hold);
    }
}
