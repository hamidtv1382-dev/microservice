using Media_Service.src._01_Domain.Core.Common;

namespace Media_Service.src._01_Domain.Core.ValueObjects
{
    public class FileName : ValueObject
    {
        public string Value { get; private set; }

        public FileName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("File name cannot be empty.", nameof(value));

            if (value.Length > 255)
                throw new ArgumentException("File name is too long.", nameof(value));

            // Basic sanitization for filename
            var invalidChars = Path.GetInvalidFileNameChars();
            if (value.IndexOfAny(invalidChars) >= 0)
                throw new ArgumentException("File name contains invalid characters.", nameof(value));

            Value = value.Trim();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
