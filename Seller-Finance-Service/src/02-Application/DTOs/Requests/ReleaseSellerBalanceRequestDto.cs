using System.ComponentModel.DataAnnotations;

namespace Seller_Finance_Service.src._02_Application.DTOs.Requests
{
    public class ReleaseSellerBalanceRequestDto
    {
        [Required]
        public Guid SellerId { get; set; }

        [Required]
        public Guid OrderId { get; set; }
    }
}
