using User_Profile_Service.src._01_Domain.Core.Enums;

namespace User_Profile_Service.src._02_Application.DTOs.Responses
{
    public class UserAddressResponseDto
    {
        public Guid Id { get; set; }
        public AddressType Type { get; set; }
        public string? Title { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string? State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool IsDefault { get; set; }
    }
}
