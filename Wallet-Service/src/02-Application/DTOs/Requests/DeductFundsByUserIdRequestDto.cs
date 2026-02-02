using System.ComponentModel.DataAnnotations;
using Wallet_Service.src._01_Domain.Core.ValueObjects;

namespace Wallet_Service.src._02_Application.DTOs.Requests
{
    public class DeductFundsByUserIdRequestDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Money Amount { get; set; }

        public string? Description { get; set; }
    }
}
