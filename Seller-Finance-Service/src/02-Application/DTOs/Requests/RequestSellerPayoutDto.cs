using System.ComponentModel.DataAnnotations;

namespace Seller_Finance_Service.src._02_Application.DTOs.Requests
{
    public class RequestSellerPayoutDto
    {
        [Required]
        public Guid SellerId { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
