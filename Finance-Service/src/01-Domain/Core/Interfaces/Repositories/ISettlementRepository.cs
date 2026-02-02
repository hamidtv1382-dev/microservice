using Finance_Service.src._01_Domain.Core.Entities;
using Finance_Service.src._01_Domain.Core.Enums;

namespace Finance_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface ISettlementRepository
    {
        Task<Settlement?> GetByIdAsync(Guid id);
        Task<IEnumerable<Settlement>> GetBySellerIdAsync(Guid sellerId);
        Task<IEnumerable<Settlement>> GetByStatusAsync(SettlementStatus status);
        Task AddAsync(Settlement settlement);
        Task UpdateAsync(Settlement settlement);
        Task DeleteAsync(Settlement settlement);
    }
}
