using Order_Service.src._01_Domain.Core.Entities;

namespace Order_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface IDiscountRepository
    {
        Task<Discount?> GetByIdAsync(Guid id);
        Task<Discount?> GetByCodeAsync(string code);
        Task<IEnumerable<Discount>> GetAllActiveAsync();
        Task AddAsync(Discount discount);
        void Update(Discount discount);
        void Delete(Discount discount);
    }
}
