using System.ComponentModel.DataAnnotations;

namespace User_Profile_Service.src._02_Application.DTOs.Requests
{
    public class UpdateUserAvatarRequestDto
    {
        [Required]
        public Guid ProfileId { get; set; }

        [Required]
        public string AvatarUrl { get; set; }
    }
}
