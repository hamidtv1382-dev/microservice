namespace Seller_Finance_Service.src._02_Application.Exceptions
{
    public class InsufficientSellerBalanceException : Exception
    {
        public InsufficientSellerBalanceException(string message) : base(message) { }
        public InsufficientSellerBalanceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
