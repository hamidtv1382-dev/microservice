using System.ComponentModel.DataAnnotations;

namespace Notification_Services.src._02_Application.DTOs.Requests
{
    public class UpdateTemplateRequestDto
    {
        [Required]
        public Guid TemplateId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Subject { get; set; }

        [Required]
        public string BodyContent { get; set; }
    }
}
