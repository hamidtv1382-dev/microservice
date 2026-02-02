using Finance_Service.src._01_Domain.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Finance_Service.src._02_Application.DTOs.Requests
{
    public class ApplyFeeRequestDto
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Money OrderAmount { get; set; }

        public Guid? SellerId { get; set; }
    }
}
