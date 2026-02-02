using Finance_Service.src._01_Domain.Core.Enums;
using Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Finance_Service.src._02_Application.DTOs.Responses
{
    public class SettlementResponseDto
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; set; }
        public Money TotalAmount { get; set; }
        public SettlementStatus Status { get; set; }
        public DateTime? SettledAt { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
