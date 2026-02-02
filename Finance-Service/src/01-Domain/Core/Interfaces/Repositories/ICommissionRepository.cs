using Finance_Service.src._01_Domain.Core.Entities;

namespace Finance_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface ICommissionRepository
    {
        Task<Commission?> GetByIdAsync(Guid id);
        Task<IEnumerable<Commission>> GetByOrderIdAsync(Guid orderId);
        Task<IEnumerable<Commission>> GetBySellerIdAsync(Guid sellerId);
        Task<IEnumerable<Commission>> GetUnsettledCommissionsAsync();
        Task AddAsync(Commission commission);
        Task UpdateAsync(Commission commission);
        Task DeleteAsync(Commission commission);
    }
}
