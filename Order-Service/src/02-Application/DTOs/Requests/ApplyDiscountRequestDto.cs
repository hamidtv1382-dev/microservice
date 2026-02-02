using System.ComponentModel.DataAnnotations;

namespace Order_Service.src._02_Application.DTOs.Requests
{
    public class ApplyDiscountRequestDto
    {
        [Required]
        public string DiscountCode { get; set; }
    }
}
