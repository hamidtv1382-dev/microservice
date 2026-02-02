using Order_Service.src._01_Domain.Core.Entities;

namespace Order_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(Guid id);
        Task<Payment?> GetByOrderIdAsync(Guid orderId);
        Task AddAsync(Payment payment);
        void Update(Payment payment);
        void Delete(Payment payment);
    }
}
