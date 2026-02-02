using Notification_Services.src._01_Domain.Core.Common;

namespace Notification_Services.src._01_Domain.Core.ValueObjects
{
    public class NotificationMessage : ValueObject
    {
        public string Value { get; private set; }

        public NotificationMessage(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Notification message cannot be empty.", nameof(value));

            if (value.Length > 5000)
                throw new ArgumentException("Notification message is too long.", nameof(value));

            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
