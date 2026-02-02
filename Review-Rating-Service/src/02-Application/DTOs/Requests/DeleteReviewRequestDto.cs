using System.ComponentModel.DataAnnotations;

namespace Review_Rating_Service.src._02_Application.DTOs.Requests
{
    public class DeleteReviewRequestDto
    {
        [Required]
        public Guid ReviewId { get; set; }
    }
}
