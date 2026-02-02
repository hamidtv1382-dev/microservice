namespace Payment_Service.src._02_Application.Exceptions
{
    public class PaymentFailedException : Exception
    {
        public PaymentFailedException(string message) : base(message) { }
        public PaymentFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
