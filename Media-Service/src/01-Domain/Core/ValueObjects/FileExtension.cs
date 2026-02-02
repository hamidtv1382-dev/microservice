using Media_Service.src._01_Domain.Core.Common;

namespace Media_Service.src._01_Domain.Core.ValueObjects
{
    public class FileExtension : ValueObject
    {
        public string Value { get; private set; }

        public FileExtension(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("File extension cannot be empty.", nameof(value));

            Value = value.TrimStart('.').ToLowerInvariant();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
