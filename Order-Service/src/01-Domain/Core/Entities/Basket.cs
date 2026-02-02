using Order_Service.src._01_Domain.Core.Common;
using Order_Service.src._01_Domain.Core.ValueObjects;

namespace Order_Service.src._01_Domain.Core.Entities
{
    public class Basket : BaseEntity
    {
        public Guid Id { get; private set; }
        public Guid BuyerId { get; private set; }
        private readonly List<BasketItem> _items;
        public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();

        public Guid? DiscountId { get; private set; }
        public Discount? AppliedDiscount { get; private set; }

        public Money TotalAmount { get; private set; }
        public DateTime? ExpiresAt { get; private set; }

        protected Basket()
        {
            _items = new List<BasketItem>();
            TotalAmount = Money.Zero();
        }

        public Basket(Guid id, Guid buyerId, TimeSpan expiration)
        {
            Id = id;
            BuyerId = buyerId;
            _items = new List<BasketItem>();
            TotalAmount = Money.Zero();
            ExpiresAt = DateTime.UtcNow.Add(expiration);
        }

        public void AddItem(BasketItem item)
        {
            var existingItem = _items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + item.Quantity);
            }
            else
            {
                _items.Add(item);
            }
            CalculateTotal();
            UpdateTimestamp();
        }

        public void RemoveItem(int productId)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                _items.Remove(item);
                CalculateTotal();
                UpdateTimestamp();
            }
        }

        public void UpdateItemQuantity(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                RemoveItem(productId);
                return;
            }

            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.UpdateQuantity(quantity);
                CalculateTotal();
                UpdateTimestamp();
            }
        }

        public void Clear()
        {
            _items.Clear();
            DiscountId = null;
            AppliedDiscount = null; // پاک کردن رفرنس در مموری
            CalculateTotal();
            UpdateTimestamp();
        }

        // فقط شناسه را ست می‌کند
        public void ApplyDiscount(Guid discountId)
        {
            DiscountId = discountId;
            UpdateTimestamp();
        }

        public void RemoveDiscount()
        {
            DiscountId = null;
            AppliedDiscount = null;
            UpdateTimestamp();
        }

        private void CalculateTotal()
        {
            TotalAmount = _items.Any() ? new Money(_items.Sum(i => i.TotalPrice.Value), "IRR") : Money.Zero();
        }

        public bool IsExpired()
        {
            return ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
        }
    }
}
