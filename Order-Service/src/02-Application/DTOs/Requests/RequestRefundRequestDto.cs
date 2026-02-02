using System.ComponentModel.DataAnnotations;

namespace Order_Service.src._02_Application.DTOs.Requests
{
    public class RequestRefundRequestDto
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Reason must be at least 10 characters")]
        public string Reason { get; set; }
    }
}
