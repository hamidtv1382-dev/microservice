namespace Payment_Service.src._02_Application.Exceptions
{
    public class RefundFailedException : Exception
    {
        public RefundFailedException(string message) : base(message) { }
        public RefundFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
