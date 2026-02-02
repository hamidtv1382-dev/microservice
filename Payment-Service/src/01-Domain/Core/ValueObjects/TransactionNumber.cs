using Payment_Service.src._01_Domain.Core.Common;

namespace Payment_Service.src._01_Domain.Core.ValueObjects
{
    public class TransactionNumber : ValueObject
    {
        public string Value { get; private set; }

        public TransactionNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Transaction number cannot be empty", nameof(value));

            Value = value;
        }

        public static TransactionNumber Generate()
        {
            return new TransactionNumber($"TRX-{Guid.NewGuid():N}");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
