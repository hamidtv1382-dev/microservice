using System.ComponentModel.DataAnnotations.Schema;
using User_Profile_Service.src._01_Domain.Core.ValueObjects;

namespace User_Profile_Service.src._01_Domain.Core.Aggregates.UserProfile
{
    [Table("UserProfiles")] // Owned type stored in Owner's table
    public class UserContactInfo
    {
        // Email Properties
        public string Email { get; private set; }

        // Phone Properties
        public string? PhoneNumber { get; private set; }
        public string? CountryCode { get; private set; }

        private UserContactInfo() { }

        public UserContactInfo(Core.ValueObjects.EmailAddress email, Core.ValueObjects.PhoneNumber? phoneNumber = null)
        {
            Email = email.Value;
            if (phoneNumber != null)
            {
                PhoneNumber = phoneNumber.Number;
                CountryCode = phoneNumber.CountryCode;
            }
        }

        public void UpdateEmail(Core.ValueObjects.EmailAddress newEmail)
        {
            Email = newEmail.Value;
        }

        public void UpdatePhoneNumber(Core.ValueObjects.PhoneNumber? newPhoneNumber)
        {
            if (newPhoneNumber != null)
            {
                PhoneNumber = newPhoneNumber.Number;
                CountryCode = newPhoneNumber.CountryCode;
            }
            else
            {
                PhoneNumber = null;
                CountryCode = null;
            }
        }
    }
}
