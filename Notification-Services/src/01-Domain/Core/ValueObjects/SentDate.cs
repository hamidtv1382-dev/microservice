using Notification_Services.src._01_Domain.Core.Common;

namespace Notification_Services.src._01_Domain.Core.ValueObjects
{
    public class SentDate : ValueObject
    {
        public DateTime Value { get; private set; }

        public SentDate()
        {
            Value = DateTime.UtcNow;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
