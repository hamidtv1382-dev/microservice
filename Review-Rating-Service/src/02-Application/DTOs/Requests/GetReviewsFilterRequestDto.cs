using Review_Rating_Service.src._01_Domain.Core.Enums;

namespace Review_Rating_Service.src._02_Application.DTOs.Requests
{
    public class GetReviewsFilterRequestDto
    {
        public int? ProductId { get; set; }
        public Guid? UserId { get; set; }
        public int? MinRating { get; set; }
        public int? MaxRating { get; set; }
        public ReviewStatus? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
