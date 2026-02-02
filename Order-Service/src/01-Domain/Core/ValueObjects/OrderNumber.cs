using Order_Service.src._01_Domain.Core.Common;

namespace Order_Service.src._01_Domain.Core.ValueObjects
{
    public class OrderNumber : ValueObject
    {
        public string Value { get; private set; }

        protected OrderNumber() { }

        public OrderNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Order number cannot be empty.", nameof(value));

            if (value.Length > 50)
                throw new ArgumentException("Order number cannot exceed 50 characters.", nameof(value));

            Value = value.Trim();
        }

        public static OrderNumber Generate()
        {
            // Standard format: ORD-{Timestamp}-{Random4Digits}
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var random = new Random().Next(1000, 9999);
            return new OrderNumber($"ORD-{timestamp}-{random}");
        }

        public static implicit operator string(OrderNumber orderNumber)
        {
            return orderNumber?.Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
