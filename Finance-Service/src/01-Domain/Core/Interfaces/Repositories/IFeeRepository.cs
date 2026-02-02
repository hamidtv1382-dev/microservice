using Finance_Service.src._01_Domain.Core.Entities;

namespace Finance_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface IFeeRepository
    {
        Task<Fee?> GetByIdAsync(Guid id);
        Task<IEnumerable<Fee>> GetByOrderIdAsync(Guid orderId);
        Task<IEnumerable<Fee>> GetBySellerIdAsync(Guid sellerId);
        Task<IEnumerable<Fee>> GetUnpaidFeesAsync();
        Task AddAsync(Fee fee);
        Task UpdateAsync(Fee fee);
        Task DeleteAsync(Fee fee);
    }
}
