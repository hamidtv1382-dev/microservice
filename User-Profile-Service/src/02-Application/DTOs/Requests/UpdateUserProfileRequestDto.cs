using System.ComponentModel.DataAnnotations;

namespace User_Profile_Service.src._02_Application.DTOs.Requests
{
    public class UpdateUserProfileRequestDto
    {
        public Guid ProfileId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }
        public string? CountryCode { get; set; } = "98";
        public DateTime? BirthDate { get; set; }
    }
}
