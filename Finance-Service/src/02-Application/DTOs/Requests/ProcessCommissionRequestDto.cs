using Finance_Service.src._01_Domain.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Finance_Service.src._02_Application.DTOs.Requests
{
    public class ProcessCommissionRequestDto
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid SellerId { get; set; }

        [Required]
        public Money SaleAmount { get; set; }
    }
}
