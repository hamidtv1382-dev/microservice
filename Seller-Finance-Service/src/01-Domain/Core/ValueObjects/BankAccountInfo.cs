using Seller_Finance_Service.src._01_Domain.Core.Common;

namespace Seller_Finance_Service.src._01_Domain.Core.ValueObjects
{
    public class BankAccountInfo : ValueObject
    {
        public string AccountNumber { get; private set; }
        public string BankName { get; private set; }
        public string ShebaNumber { get; private set; }
        public string AccountHolderName { get; private set; }

        public BankAccountInfo(string accountNumber, string bankName, string shebaNumber, string accountHolderName)
        {
            if (string.IsNullOrWhiteSpace(accountNumber)) throw new ArgumentException("Account number is required");
            if (string.IsNullOrWhiteSpace(shebaNumber)) throw new ArgumentException("Sheba number is required");

            AccountNumber = accountNumber;
            BankName = bankName;
            ShebaNumber = shebaNumber;
            AccountHolderName = accountHolderName;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AccountNumber;
            yield return ShebaNumber;
        }
    }
}
