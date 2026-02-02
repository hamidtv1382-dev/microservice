using Review_Rating_Service.src._01_Domain.Core.Common;

namespace Review_Rating_Service.src._01_Domain.Core.ValueObjects
{
    public class ReviewDate : ValueObject
    {
        public DateTime Value { get; private set; }

        public ReviewDate(DateTime value)
        {
            if (value > DateTime.UtcNow)
                throw new ArgumentException("Review date cannot be in the future.", nameof(value));

            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
