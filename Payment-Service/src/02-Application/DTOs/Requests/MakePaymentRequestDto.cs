using System.ComponentModel.DataAnnotations;

namespace Payment_Service.src._02_Application.DTOs.Requests
{
    public class MakePaymentRequestDto
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public string CallbackUrl { get; set; }
    }
}