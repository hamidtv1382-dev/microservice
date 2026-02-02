using Review_Rating_Service.src._01_Domain.Core.Enums;

namespace Review_Rating_Service.src._02_Application.DTOs.Responses
{
    public class RatingResponseDto
    {
        public Guid Id { get; set; }
        public RatingType Type { get; set; }
        public int Value { get; set; }
    }
}
