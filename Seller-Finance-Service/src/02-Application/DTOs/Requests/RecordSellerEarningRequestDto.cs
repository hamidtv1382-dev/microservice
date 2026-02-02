using System.ComponentModel.DataAnnotations;

namespace Seller_Finance_Service.src._02_Application.DTOs.Requests
{
    public class RecordSellerEarningRequestDto
    {
        [Required]
        public Guid SellerId { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
