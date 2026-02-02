using System.ComponentModel.DataAnnotations;

namespace Seller_Finance_Service.src._02_Application.DTOs.Requests
{
    public class GetSellerTransactionsRequestDto
    {
        [Required]
        public Guid SellerId { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
