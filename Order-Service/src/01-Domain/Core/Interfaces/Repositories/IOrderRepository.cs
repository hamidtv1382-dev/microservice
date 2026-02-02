using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.Enums;

namespace Order_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id);
        Task<Order?> GetByOrderNumberAsync(string orderNumber);
        Task<IEnumerable<Order>> GetByBuyerIdAsync(Guid buyerId);
        Task<IEnumerable<Order>> GetAllAsync();
        Task AddAsync(Order order);
        void Update(Order order);
        void Delete(Order order);
    }
}
