using Payment_Service.src._01_Domain.Core.Entities;
using Payment_Service.src._01_Domain.Core.Enums;

namespace Payment_Service.src._01_Domain.Core.Interfaces.Repositories
{
    public interface IRefundRepository
    {
        Task<Refund?> GetByIdAsync(Guid id);
        Task<IEnumerable<Refund>> GetByPaymentIdAsync(Guid paymentId);
        Task<IEnumerable<Refund>> GetByStatusAsync(RefundStatus status);
        Task AddAsync(Refund refund);
        Task UpdateAsync(Refund refund);
        Task DeleteAsync(Refund refund);
    }
}
