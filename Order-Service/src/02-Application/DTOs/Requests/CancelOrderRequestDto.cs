using System.ComponentModel.DataAnnotations;

namespace Order_Service.src._02_Application.DTOs.Requests
{
    public class CancelOrderRequestDto
    {
        [Required]
        public Guid OrderId { get; set; }

        public string? Reason { get; set; }
    }
}
