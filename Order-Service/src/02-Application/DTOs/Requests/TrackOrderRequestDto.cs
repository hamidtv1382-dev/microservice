using System.ComponentModel.DataAnnotations;

namespace Order_Service.src._02_Application.DTOs.Requests
{
    public class TrackOrderRequestDto
    {
        [Required]
        public Guid OrderId { get; set; }
    }
}
