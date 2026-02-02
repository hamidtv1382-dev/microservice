using Order_Service.src._01_Domain.Core.Entities;

namespace Order_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface IRefundRepository
    {
        Task<Refund?> GetByIdAsync(Guid id);
        Task<Refund?> GetByOrderIdAsync(Guid orderId);
        Task<IEnumerable<Refund>> GetByPaymentIdAsync(Guid paymentId);
        Task AddAsync(Refund refund);
        void Update(Refund refund);
        void Delete(Refund refund);
    }
}
