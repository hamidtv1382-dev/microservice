using Seller_Finance_Service.src._01_Domain.Core.Entities;
using Seller_Finance_Service.src._01_Domain.Core.Enums;
using Seller_Finance_Service.src._01_Domain.Core.Interfaces.UnitOfWork;
using Seller_Finance_Service.src._01_Domain.Core.ValueObjects;
using Seller_Finance_Service.src._01_Domain.Services.Interfaces;

namespace Seller_Finance_Service.src._01_Domain.Services.Implementations
{
    public class SellerFinanceDomainService : ISellerFinanceDomainService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SellerFinanceDomainService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SellerAccount> CreateAccountAsync(Guid sellerId, BankAccountInfo bankAccount)
        {
            var account = new SellerAccount(sellerId, bankAccount, "System");
            await _unitOfWork.SellerAccounts.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();
            return account;
        }

        public async Task<SellerAccount?> GetAccountAsync(Guid sellerId)
        {
            return await _unitOfWork.SellerAccounts.GetBySellerIdAsync(sellerId);
        }

        public async Task<bool> RecordEarningAsync(Guid sellerId, Guid orderId, Money amount)
        {
            var account = await _unitOfWork.SellerAccounts.GetBySellerIdAsync(sellerId);
            if (account == null) return false;

            // 1. Add to Pending Balance
            account.Balance.AddToPending(amount);

            // 2. Create Transaction Record
            var transaction = new SellerTransaction(
                account.Id,
                amount,
                SellerTransactionType.SaleEarning,
                orderId.ToString(),
                "Sale Earning",
                "System"
            );

            await _unitOfWork.SellerTransactions.AddAsync(transaction);
            _unitOfWork.SellerAccounts.UpdateAsync(account);

            // 3. Add Domain Event
            account.AddDomainEvent(new Core.Events.SellerEarningRecordedEvent(account.Id, orderId, amount.Amount));

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReleaseBalanceAsync(Guid sellerId, Guid orderId)
        {
            var account = await _unitOfWork.SellerAccounts.GetBySellerIdAsync(sellerId);
            if (account == null) return false;

            var transactions = await _unitOfWork.SellerTransactions.GetByReferenceIdAsync(orderId.ToString());
            var earningTx = transactions.FirstOrDefault(t => t.Type == SellerTransactionType.SaleEarning && t.Status == SellerTransactionStatus.Success);

            if (earningTx == null) return false;

            // Move from Pending to Available
            account.Balance.ReleaseFromPendingToAvailable(earningTx.Amount);

            // Create Release Transaction
            var releaseTx = new SellerTransaction(
                account.Id,
                earningTx.Amount,
                SellerTransactionType.BalanceRelease,
                orderId.ToString(),
                "Balance Released",
                "System"
            );

            await _unitOfWork.SellerTransactions.AddAsync(releaseTx);
            _unitOfWork.SellerAccounts.UpdateAsync(account);

            // Add Domain Event
            account.AddDomainEvent(new Core.Events.SellerBalanceReleasedEvent(account.Id, orderId, earningTx.Amount.Amount));

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HoldBalanceAsync(Guid sellerId, Money amount, HoldReasonType reason, string description)
        {
            var account = await _unitOfWork.SellerAccounts.GetBySellerIdAsync(sellerId);
            if (account == null) return false;

            // Check if enough in Available
            if (account.Balance.AvailableBalance.Amount < amount.Amount) return false;

            // Move from Available to Hold
            account.Balance.DeductFromAvailable(amount);
            account.Balance.AddToHold(amount);

            // Create Hold Transaction
            var tx = new SellerTransaction(
                account.Id,
                amount,
                SellerTransactionType.Hold,
                Guid.NewGuid().ToString(),
                description,
                "System"
            );

            // Create Hold Record
            var hold = new SellerHold(account.Id, amount, reason, description, "System");

            await _unitOfWork.SellerTransactions.AddAsync(tx);
            await _unitOfWork.SellerHolds.AddAsync(hold);
            _unitOfWork.SellerAccounts.UpdateAsync(account);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
