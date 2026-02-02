using Order_Service.src._01_Domain.Core.Common;
using Order_Service.src._01_Domain.Core.Enums;
using Order_Service.src._01_Domain.Core.ValueObjects;

namespace Order_Service.src._01_Domain.Core.Entities
{
    public class Order : BaseEntity
    {
        public Guid Id { get; private set; }
        public OrderNumber OrderNumber { get; private set; }
        public Guid BuyerId { get; private set; }
        public ShippingAddress ShippingAddress { get; private set; }
        private readonly List<OrderItem> _items;
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
        public Money TotalAmount { get; private set; }
        public Money DiscountAmount { get; private set; }
        public Money FinalAmount { get; private set; }
        public OrderStatus Status { get; private set; }
        public Guid? DiscountId { get; private set; }
        public DateTime? OrderDate { get; set; }
        public string? Description { get; private set; }

        protected Order()
        {
            _items = new List<OrderItem>();
        }

        public Order(Guid id, Guid buyerId, OrderNumber orderNumber, ShippingAddress shippingAddress)
        {
            Id = id;
            BuyerId = buyerId;
            OrderNumber = orderNumber;
            ShippingAddress = shippingAddress;
            _items = new List<OrderItem>();
            Status = OrderStatus.Pending;
            TotalAmount = Money.Zero();
            DiscountAmount = Money.Zero();
            FinalAmount = Money.Zero();
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateShippingAddress(ShippingAddress newAddress)
        {
            if (Status == OrderStatus.Shipped || Status == OrderStatus.Delivered)
                throw new InvalidOperationException("Cannot change address for shipped or delivered orders.");

            ShippingAddress = newAddress;
            UpdateTimestamp();
        }

        public void MarkAsRefunded()
        {
            Status = OrderStatus.Refunded;
        }
        public void AddItem(OrderItem item)
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Cannot add items to an order that is not in Pending status.");

            var existingItem = _items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + item.Quantity);
            }
            else
            {
                _items.Add(item);
            }
            RecalculateTotals();
        }

        public void RemoveItem(Guid itemId)
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Cannot remove items from an order that is not in Pending status.");

            var item = _items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                _items.Remove(item);
                RecalculateTotals();
            }
        }

        public void ApplyDiscount(Money discountAmount, Guid discountId)
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Cannot apply discount to an order that is not in Pending status.");

            if (discountAmount.Value <= 0)
                throw new ArgumentException("Discount amount must be greater than zero.");

            DiscountId = discountId;
            DiscountAmount = discountAmount;
            RecalculateTotals();
        }

        public void Confirm()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Cannot confirm an order with no items.");

            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Order is not in Pending status.");

            Status = OrderStatus.Confirmed;
            OrderDate = DateTime.UtcNow;
            UpdateTimestamp();
        }

        public void Ship()
        {
            if (Status != OrderStatus.Confirmed)
                throw new InvalidOperationException("Order is not in Confirmed status.");

            Status = OrderStatus.Shipped;
            UpdateTimestamp();
        }

        public void Deliver()
        {
            if (Status != OrderStatus.Shipped)
                throw new InvalidOperationException("Order is not in Shipped status.");

            Status = OrderStatus.Delivered;
            UpdateTimestamp();
        }

        public void Cancel(string reason = null)
        {
            if (Status == OrderStatus.Delivered || Status == OrderStatus.Cancelled || Status == OrderStatus.Refunded)
                throw new InvalidOperationException("Cannot cancel this order.");

            Status = OrderStatus.Cancelled;
            Description = reason;
            UpdateTimestamp();
        }

        private void RecalculateTotals()
        {
            var total = _items.Sum(i => i.TotalPrice.Value);
            TotalAmount = new Money(total);
            FinalAmount = new Money(total - DiscountAmount.Value);
        }
    }
}