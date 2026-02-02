using Review_Rating_Service.src._01_Domain.Core.Common;

namespace Review_Rating_Service.src._01_Domain.Core.ValueObjects
{
    public class ReviewText : ValueObject
    {
        public string Value { get; private set; }

        public ReviewText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Review text cannot be empty.", nameof(value));

            if (value.Length > 2000)
                throw new ArgumentException("Review text is too long.", nameof(value));

            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
