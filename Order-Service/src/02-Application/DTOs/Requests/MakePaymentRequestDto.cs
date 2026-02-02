using System.ComponentModel.DataAnnotations;

namespace Order_Service.src._02_Application.DTOs.Requests
{
    public class MakePaymentRequestDto
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        public string Cvv2 { get; set; }

        [Required]
        public string ExpMonth { get; set; }

        [Required]
        public string ExpYear { get; set; }

        [Required]
        public string Pin { get; set; }
    }
}
