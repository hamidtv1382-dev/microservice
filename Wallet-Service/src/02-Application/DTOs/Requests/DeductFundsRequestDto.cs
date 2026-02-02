using System.ComponentModel.DataAnnotations;
using Wallet_Service.src._01_Domain.Core.ValueObjects;

namespace Wallet_Service.src._02_Application.DTOs.Requests
{
    public class DeductFundsRequestDto
    {
        [Required]
        public Guid WalletId { get; set; }

        [Required]
        public Money Amount { get; set; }

        public string Description { get; set; }
    }
}
