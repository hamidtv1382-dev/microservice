using Order_Service.src._01_Domain.Core.Entities;
using Order_Service.src._01_Domain.Core.Enums;
using Order_Service.src._01_Domain.Core.Interfaces.Repositories;
using Order_Service.src._01_Domain.Core.ValueObjects;
using Order_Service.src._01_Domain.Services.Interfaces;

namespace Order_Service.src._01_Domain.Services.Implementations
{
    public class OrderDomainService : IOrderDomainService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDiscountService _discountService;
        private readonly IInventoryService _inventoryService;

        public OrderDomainService(IOrderRepository orderRepository, IDiscountService discountService, IInventoryService inventoryService)
        {
            _orderRepository = orderRepository;
            _discountService = discountService;
            _inventoryService = inventoryService;
        }

        public async Task<Order> CreateOrderAsync(Guid buyerId, ShippingAddress shippingAddress, List<OrderItem> items, string? discountCode = null)
        {
            if (items == null || !items.Any())
                throw new ArgumentException("Order must contain at least one item.");

            var orderNumber = OrderNumber.Generate();
            var order = new Order(Guid.NewGuid(), buyerId, orderNumber, shippingAddress);

            foreach (var item in items)
            {
                // Verify stock before adding
                var isInStock = await _inventoryService.CheckStockAsync(item.ProductId, item.Quantity);
                if (!isInStock)
                    throw new InvalidOperationException($"Insufficient stock for product {item.ProductName}.");

                order.AddItem(item);
            }

            // Apply discount if code provided
            if (!string.IsNullOrWhiteSpace(discountCode))
            {
                var discount = await _discountService.ValidateDiscountAsync(discountCode, order.TotalAmount);
                if (discount != null)
                {
                    var discountAmount = _discountService.CalculateDiscountAmount(discount, order.TotalAmount);
                    order.ApplyDiscount(discountAmount, discount.Id);
                }
            }

            return order;
        }

        public async Task<bool> CanCancelOrderAsync(Order order)
        {
            if (order.Status == OrderStatus.Cancelled || order.Status == OrderStatus.Refunded || order.Status == OrderStatus.Delivered)
                return false;

            // Business rule: Can cancel if not shipped yet, or within 1 hour of shipping (example logic)
            if (order.Status == OrderStatus.Shipped)
            {
                // Assuming OrderDate is set when shipped or created
                if (order.OrderDate.HasValue && (DateTime.UtcNow - order.OrderDate.Value).TotalHours > 1)
                    return false;
            }

            return true;
        }

        public void CancelOrder(Order order, string reason)
        {
            if (!CanCancelOrderAsync(order).GetAwaiter().GetResult())
                throw new InvalidOperationException("Order cannot be cancelled at this stage.");

            order.Cancel(reason);
        }
    }
}