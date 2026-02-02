using Review_Rating_Service.src._01_Domain.Core.Enums;

namespace Review_Rating_Service.src._02_Application.DTOs.Responses
{
    public class ReviewResponseDto
    {
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public Guid UserId { get; set; }
        public string ReviewerName { get; set; }
        public string Text { get; set; }
        public DateTime ReviewDate { get; set; }
        public ReviewStatus Status { get; set; }
        public List<RatingResponseDto> Ratings { get; set; }

        // این فیلد از قبل وجود داشت و فقط لیستی از URL ها را برمی‌گرداند
        public List<string> AttachmentUrls { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}