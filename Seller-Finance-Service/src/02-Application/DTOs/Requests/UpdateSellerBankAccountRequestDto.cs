using System.ComponentModel.DataAnnotations;

namespace Seller_Finance_Service.src._02_Application.DTOs.Requests
{
    public class UpdateSellerBankAccountRequestDto
    {
        [Required]
        public Guid SellerId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string BankName { get; set; }

        [Required]
        [StringLength(26, MinimumLength = 24)]
        public string ShebaNumber { get; set; }

        [Required]
        [StringLength(200)]
        public string AccountHolderName { get; set; }
    }
}
