using Review_Rating_Service.src._01_Domain.Core.Common;

namespace Review_Rating_Service.src._01_Domain.Core.ValueObjects
{
    public class RatingValue : ValueObject
    {
        public int Value { get; private set; }

        public RatingValue(int value)
        {
            if (value < 1 || value > 5)
                throw new ArgumentException("Rating must be between 1 and 5.", nameof(value));

            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
