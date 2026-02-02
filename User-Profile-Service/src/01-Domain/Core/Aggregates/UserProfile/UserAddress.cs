using System.ComponentModel.DataAnnotations.Schema;
using User_Profile_Service.src._01_Domain.Core.Enums;
using User_Profile_Service.src._01_Domain.Core.ValueObjects;

namespace User_Profile_Service.src._01_Domain.Core.Aggregates.UserProfile
{
    [Table("UserAddresses")]
    public class UserAddress
    {
        public Guid Id { get; private set; }
        public AddressType Type { get; private set; }

        // پراپرتی‌های Address مستقیماً اینجا قرار می‌گیرند (Flattened)
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string PostalCode { get; private set; }
        public string Country { get; private set; }

        public string? Title { get; private set; }
        public bool IsDefault { get; private set; }

        // سازنده خصوصی برای EF Core
        private UserAddress() { }

        // سازخانه عمومی برای Domain
        public UserAddress(Guid id, AddressType type, Core.ValueObjects.Address address, string? title = null)
        {
            Id = id;
            Type = type;
            Street = address.Street;
            City = address.City;
            State = address.State;
            PostalCode = address.PostalCode;
            Country = address.Country;
            Title = title;
        }

        public void SetAsDefault()
        {
            IsDefault = true;
        }

        public void UnsetDefault()
        {
            IsDefault = false;
        }
    }
}
