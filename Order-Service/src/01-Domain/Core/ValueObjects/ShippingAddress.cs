using Order_Service.src._01_Domain.Core.Common;

namespace Order_Service.src._01_Domain.Core.ValueObjects
{
    public class ShippingAddress : ValueObject
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Country { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Street { get; private set; }
        public string ZipCode { get; private set; }
        public string? BuildingNumber { get; private set; }
        public string? ApartmentNumber { get; private set; }
        public string? PostalCodeAdditional { get; private set; }

        protected ShippingAddress() { }

        public ShippingAddress(string firstName, string lastName, string phoneNumber, string country, string city, string state, string street, string zipCode, string? buildingNumber = null, string? apartmentNumber = null, string? postalCodeAdditional = null)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.", nameof(lastName));
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number is required.", nameof(phoneNumber));
            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentException("Country is required.", nameof(country));
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City is required.", nameof(city));
            if (string.IsNullOrWhiteSpace(state))
                throw new ArgumentException("State is required.", nameof(state));
            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Street is required.", nameof(street));
            if (string.IsNullOrWhiteSpace(zipCode))
                throw new ArgumentException("Zip code is required.", nameof(zipCode));

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            PhoneNumber = phoneNumber.Trim();
            Country = country.Trim();
            City = city.Trim();
            State = state.Trim();
            Street = street.Trim();
            ZipCode = zipCode.Trim();
            BuildingNumber = buildingNumber?.Trim();
            ApartmentNumber = apartmentNumber?.Trim();
            PostalCodeAdditional = postalCodeAdditional?.Trim();
        }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        public string GetFullAddress()
        {
            var address = $"{Street}, {City}, {State}, {Country}, {ZipCode}";

            if (!string.IsNullOrEmpty(BuildingNumber))
                address = $"Building {BuildingNumber}, " + address;

            if (!string.IsNullOrEmpty(ApartmentNumber))
                address = $"Apt {ApartmentNumber}, " + address;

            return address;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
            yield return PhoneNumber;
            yield return Country;
            yield return City;
            yield return State;
            yield return Street;
            yield return ZipCode;
            yield return BuildingNumber;
            yield return ApartmentNumber;
            yield return PostalCodeAdditional;
        }
    }
}
