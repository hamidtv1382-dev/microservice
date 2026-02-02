using Notification_Services.src._01_Domain.Core.Common;

namespace Notification_Services.src._01_Domain.Core.ValueObjects
{
    public class NotificationTitle : ValueObject
    {
        public string Value { get; private set; }

        public NotificationTitle(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Notification title cannot be empty.", nameof(value));

            if (value.Length > 200)
                throw new ArgumentException("Notification title is too long.", nameof(value));

            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
