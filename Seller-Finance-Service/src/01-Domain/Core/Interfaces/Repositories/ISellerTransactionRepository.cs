using Seller_Finance_Service.src._01_Domain.Core.Entities;

namespace Seller_Finance_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface ISellerTransactionRepository
    {
        Task<SellerTransaction?> GetByIdAsync(Guid id);
        Task<IEnumerable<SellerTransaction>> GetBySellerAccountIdAsync(Guid accountId);
        Task<IEnumerable<SellerTransaction>> GetByReferenceIdAsync(string referenceId);
        Task AddAsync(SellerTransaction transaction);
        Task UpdateAsync(SellerTransaction transaction);
        Task DeleteAsync(SellerTransaction transaction);
    }
}
