using Seller_Finance_Service.src._01_Domain.Core.Entities;
using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Seller_Finance_Service.src._01_Domain.Services.Interfaces
{
    public interface IPayoutPolicyService
    {
        bool CanRequestPayout(SellerAccount account, Money requestedAmount);
        SellerPayout CreatePayout(SellerAccount account, Money amount);
    }
}
