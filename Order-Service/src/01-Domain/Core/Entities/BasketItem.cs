using Order_Service.src._01_Domain.Core.Common;
using Order_Service.src._01_Domain.Core.ValueObjects;

namespace Order_Service.src._01_Domain.Core.Entities
{
    public class BasketItem : BaseEntity
    {
        public Guid Id { get; private set; }
        public Guid BasketId { get; private set; }
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string? ImageUrl { get; private set; }
        public Money UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public Money TotalPrice => UnitPrice * Quantity;

        protected BasketItem() { }

        public BasketItem(Guid id, Guid basketId, int productId, string productName, string? imageUrl, Money unitPrice, int quantity)
        {
            Id = id;
            BasketId = basketId;
            ProductId = productId;
            ProductName = productName;
            ImageUrl = imageUrl;
            UnitPrice = unitPrice;
            SetQuantity(quantity);
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            Quantity = quantity;
            UpdateTimestamp();
        }

        public void UpdatePrice(Money newPrice)
        {
            UnitPrice = newPrice;
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
