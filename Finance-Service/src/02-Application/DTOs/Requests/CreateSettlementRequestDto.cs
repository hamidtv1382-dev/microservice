using Finance_Service.src._01_Domain.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Finance_Service.src._02_Application.DTOs.Requests
{
    public class CreateSettlementRequestDto
    {
        [Required]
        public Guid SellerId { get; set; }

        [Required]
        public Money TotalAmount { get; set; } // اضافه شد: مبلغ تسویه

        public string? BankAccountInfo { get; set; } // اضافه شد: شماره شبا

        public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(7);
    }
}
