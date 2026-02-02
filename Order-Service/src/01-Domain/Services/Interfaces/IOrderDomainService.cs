using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.ValueObjects;

namespace Order_Service.src._01_Domain.Services.Interfaces
{
    public interface IOrderDomainService
    {
        Task<Order> CreateOrderAsync(Guid buyerId, ShippingAddress shippingAddress, List<OrderItem> items, string? discountCode = null);
        Task<bool> CanCancelOrderAsync(Order order);
        void CancelOrder(Order order, string reason);
    }
}
