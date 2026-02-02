using System.ComponentModel.DataAnnotations;

namespace Review_Rating_Service.src._02_Application.DTOs.Requests
{
    public class UpdateReviewRequestDto
    {
        [Required]
        public Guid ReviewId { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Text { get; set; }
    }
}
