namespace Seller_Finance_Service.src._02_Application.Exceptions
{
    public class SellerBalanceHoldException : Exception
    {
        public SellerBalanceHoldException(string message) : base(message) { }
        public SellerBalanceHoldException(string message, Exception innerException) : base(message, innerException) { }
    }
}
