using Payment_Service.src._01_Domain.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Payment_Service.src._02_Application.DTOs.Requests
{
    public class RequestRefundRequestDto
    {
        [Required]
        public Guid PaymentId { get; set; }

        [Required]
        public Money Amount { get; set; }

        [Required]
        [MinLength(10)]
        public string Reason { get; set; }
    }
}
