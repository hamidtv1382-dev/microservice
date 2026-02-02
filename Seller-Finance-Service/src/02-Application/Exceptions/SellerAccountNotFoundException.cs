namespace Seller_Finance_Service.src._02_Application.Exceptions
{
    public class SellerAccountNotFoundException : Exception
    {
        public SellerAccountNotFoundException(string message) : base(message) { }
        public SellerAccountNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
