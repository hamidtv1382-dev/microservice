using Messaging_Service.src._01_Domain.Core.Common;

namespace Messaging_Service.src._01_Domain.Core.ValueObjects
{
    public class ContactInfo : ValueObject
    {
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }

        public ContactInfo(string email, string phoneNumber)
        {
            Email = email;
            PhoneNumber = phoneNumber;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Email;
            yield return PhoneNumber;
        }
    }
}
