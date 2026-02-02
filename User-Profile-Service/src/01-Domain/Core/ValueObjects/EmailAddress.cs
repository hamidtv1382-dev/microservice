using User_Profile_Service.src._01_Domain.Core.Common;

namespace User_Profile_Service.src._01_Domain.Core.ValueObjects
{
    public class EmailAddress : ValueObject
    {
        public string Value { get; private set; }

        public EmailAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email is required.", nameof(value));

            if (!value.Contains('@'))
                throw new ArgumentException("Invalid email format.", nameof(value));

            Value = value.Trim().ToLower();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
