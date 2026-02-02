using Finance_Service.src._01_Domain.Core.Enums;
using Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Finance_Service.src._02_Application.DTOs.Responses
{
    public class FeeResponseDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Money Amount { get; set; }
        public FeeType Type { get; set; }
        public string Description { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
