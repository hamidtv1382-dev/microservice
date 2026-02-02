using Seller_Finance_Service.src._01_Domain.Core.Common;
using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;

namespace Seller_Finance_Service.src._01_Domain.Core.Entities
{
    public class SellerAccount : AggregateRoot
    {
        public Guid SellerId { get; private set; }

        // Backing Field initialized to prevent NullReferenceException
        private SellerBalance _balance = new SellerBalance();
        public SellerBalance Balance => _balance;

        public BankAccountInfo BankAccount { get; private set; }
        public bool IsActive { get; private set; }
        public AuditInfo AuditInfo { get; private set; }

        private SellerAccount() { }

        public SellerAccount(Guid sellerId, BankAccountInfo bankAccount, string createdBy)
        {
            Id = Guid.NewGuid();
            SellerId = sellerId;
            // _balance is already initialized by field initializer
            BankAccount = bankAccount;
            IsActive = true;
            AuditInfo = new AuditInfo(createdBy);
        }

        public void UpdateBankAccount(BankAccountInfo newInfo)
        {
            BankAccount = newInfo;
            SetUpdatedAt();
        }

        public void Activate()
        {
            IsActive = true;
            SetUpdatedAt();
        }

        public void Deactivate()
        {
            IsActive = false;
            SetUpdatedAt();
        }
    }
}