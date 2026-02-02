using Seller_Finance_Service.src._01_Domain.Core.Common;

namespace Seller_Finance_Service.src._01_Domain.Core.ValueObjects
{
    public class Percentage : ValueObject
    {
        public decimal Value { get; private set; }

        public Percentage(decimal value)
        {
            if (value < 0 || value > 100)
                throw new ArgumentException("Percentage must be between 0 and 100", nameof(value));

            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
