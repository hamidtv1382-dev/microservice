using Seller_Finance_Service.src._01_Domain.Core.Entities;

namespace Seller_Finance_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface ISellerAccountRepository
    {
        Task<SellerAccount?> GetByIdAsync(Guid id);
        Task<SellerAccount?> GetBySellerIdAsync(Guid sellerId);
        Task AddAsync(SellerAccount account);
        Task UpdateAsync(SellerAccount account);
        Task DeleteAsync(SellerAccount account);
    }
}
