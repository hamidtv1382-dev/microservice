namespace Seller_Finance_Service.src._02_Application.Exceptions
{
    public class SellerPayoutFailedException : Exception
    {
        public SellerPayoutFailedException(string message) : base(message) { }
        public SellerPayoutFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
