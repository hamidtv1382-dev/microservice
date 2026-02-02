using Order_Service.src._01_Domain.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Order_Service.src._02_Application.DTOs.Requests
{
    public class CreateDiscountRequestDto
    {
        [Required(ErrorMessage = "Discount code is required.")]
        [StringLength(50, ErrorMessage = "Code cannot be longer than 50 characters.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Discount type is required.")]
        public DiscountType Type { get; set; }

        [Required(ErrorMessage = "Discount value is required.")]
        [Range(0.01, 100000000, ErrorMessage = "Value must be greater than zero.")]
        public decimal Value { get; set; }

        [Required(ErrorMessage = "Minimum order amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Minimum order amount cannot be negative.")]
        public decimal MinimumOrderAmount { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Usage limit is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Usage limit must be at least 1.")]
        public int UsageLimit { get; set; }
    }
}
