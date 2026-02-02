using System.ComponentModel.DataAnnotations;
using User_Profile_Service.src._01_Domain.Core.Enums;

namespace User_Profile_Service.src._02_Application.DTOs.Requests
{
    public class AddUserAddressRequestDto
    {
        [Required]
        public Guid ProfileId { get; set; }

        [Required]
        public AddressType Type { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        public string? State { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Country { get; set; }

        public string? Title { get; set; }
        public bool IsDefault { get; set; }
    }
}
