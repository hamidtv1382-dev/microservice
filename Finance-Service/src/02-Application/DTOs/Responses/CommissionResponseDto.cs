using Finance_Service.src._01_Domain.Core.Enums;
using Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Finance_Service.src._02_Application.DTOs.Responses
{
    public class CommissionResponseDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid SellerId { get; set; }
        public Money Amount { get; set; }
        public CommissionType Type { get; set; }
        public Money RatePercentage { get; set; }
        public bool IsSettled { get; set; }
        public DateTime? SettledAt { get; set; }
    }
}
