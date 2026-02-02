using Review_Rating_Service.src._01_Domain.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Review_Rating_Service.src._02_Application.DTOs.Requests
{
    public class AddRatingRequestDto
    {
        [Required]
        public Guid ReviewId { get; set; }

        [Required]
        public RatingType Type { get; set; }

        [Required]
        [Range(1, 5)]
        public int Value { get; set; }
    }
}
