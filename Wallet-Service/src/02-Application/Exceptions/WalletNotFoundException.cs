namespace Wallet_Service.src._02_Application.Exceptions
{
    public class WalletNotFoundException : Exception
    {
        public WalletNotFoundException(string message) : base(message) { }
        public WalletNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
