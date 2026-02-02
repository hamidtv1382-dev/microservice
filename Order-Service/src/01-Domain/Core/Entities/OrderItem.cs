using Order_Service.src._01_Domain.Core.Common;
using Order_Service.src._01_Domain.Core.ValueObjects;

namespace Order_Service.src._01_Domain.Core.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string? ImageUrl { get; private set; }
        public Money UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public string SellerId { get; private set; }
        public Money TotalPrice => UnitPrice * Quantity;

        protected OrderItem() { }

        public OrderItem(Guid id, Guid orderId, int productId, string productName, string? imageUrl, Money unitPrice, int quantity, string sellerId)
        {
            Id = id;
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            ImageUrl = imageUrl;
            UnitPrice = unitPrice;
            SellerId = sellerId;
            SetQuantity(quantity);
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            Quantity = quantity;
            UpdateTimestamp();
        }

        public void ChangePrice(Money newUnitPrice)
        {
            UnitPrice = newUnitPrice;
            UpdateTimestamp();
        }

        private void SetQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            Quantity = quantity;
        }
    }
}