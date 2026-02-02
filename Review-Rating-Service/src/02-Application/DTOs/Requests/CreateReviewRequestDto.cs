using Review_Rating_Service.src._01_Domain.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Review_Rating_Service.src._02_Application.DTOs.Requests
{
    public class CreateReviewRequestDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Text { get; set; }

        [Required]
        [MaxLength(100)]
        public string ReviewerName { get; set; }

        public List<CreateRatingDto> Ratings { get; set; } = new List<CreateRatingDto>();

        // اضافه شده: لیست ضمیمه ها
        public List<CreateAttachmentDto> Attachments { get; set; } = new List<CreateAttachmentDto>();
    }

    public class CreateRatingDto
    {
        [Required]
        public RatingType Type { get; set; }

        [Required]
        [Range(1, 5)]
        public int Value { get; set; }
    }

    // اضافه شده: مدل برای دریافت ضمیمه
    public class CreateAttachmentDto
    {
        [Required]
        public string Url { get; set; }

        [Required]
        public AttachmentType Type { get; set; } // Image یا Video
    }
}