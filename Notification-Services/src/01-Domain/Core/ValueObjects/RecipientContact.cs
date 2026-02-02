using Notification_Services.src._01_Domain.Core.Common;

namespace Notification_Services.src._01_Domain.Core.ValueObjects
{
    public class RecipientContact : ValueObject
    {
        public string Address { get; private set; }

        public RecipientContact(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Recipient address is required.", nameof(address));

            Address = address;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address;
        }
    }
}
