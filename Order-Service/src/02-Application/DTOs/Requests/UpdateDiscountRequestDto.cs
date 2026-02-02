using Order_Service.src._01_Domain.Core.Enums;

namespace Order_Service.src._02_Application.DTOs.Requests
{
    public class UpdateDiscountRequestDto
    {
        public string? Description { get; set; }

        public DiscountType? Type { get; set; }

        public decimal? Value { get; set; }

        public decimal? MinimumOrderAmount { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? UsageLimit { get; set; }

        public bool? IsActive { get; set; }
    }
}
