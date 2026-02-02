using Media_Service.src._01_Domain.Core.Common;

namespace Media_Service.src._01_Domain.Core.ValueObjects
{
    public class FileSize : ValueObject
    {
        public long Bytes { get; private set; }

        public FileSize(long bytes)
        {
            if (bytes < 0)
                throw new ArgumentException("File size cannot be negative.", nameof(bytes));

            Bytes = bytes;
        }

        public FileSize(string size)
        {
            // Logic to parse string to bytes if needed, or use long constructor
            if (!long.TryParse(size, out long result))
                throw new ArgumentException("Invalid file size format.", nameof(size));

            Bytes = result;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Bytes;
        }
    }
}
