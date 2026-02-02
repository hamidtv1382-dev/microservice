using Review_Rating_Service.src._01_Domain.Core.Common;

namespace Review_Rating_Service.src._01_Domain.Core.ValueObjects
{
    public class ReviewerName : ValueObject
    {
        public string Value { get; private set; }

        public ReviewerName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Reviewer name cannot be empty.", nameof(value));

            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
