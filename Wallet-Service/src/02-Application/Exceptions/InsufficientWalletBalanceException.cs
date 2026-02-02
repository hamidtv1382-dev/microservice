namespace Wallet_Service.src._02_Application.Exceptions
{
    public class InsufficientWalletBalanceException : Exception
    {
        public InsufficientWalletBalanceException(string message) : base(message) { }
        public InsufficientWalletBalanceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
