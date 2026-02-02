using User_Profile_Service.src._01_Domain.Core.Common;

namespace User_Profile_Service.src._01_Domain.Core.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        public string Number { get; private set; }
        public string CountryCode { get; private set; }

        public PhoneNumber(string number, string countryCode = "98")
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("Phone number is required.", nameof(number));

            Number = number.Trim();
            CountryCode = countryCode.Trim();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CountryCode;
            yield return Number;
        }
    }
}
