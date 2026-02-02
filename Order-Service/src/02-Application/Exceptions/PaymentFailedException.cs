namespace Order_Service.src._02_Application.Exceptions
{
    public class PaymentFailedException : Exception
    {
        public string? Reason { get; }

        public PaymentFailedException(string? reason = null)
            : base($"Payment processing failed. Reason: {reason ?? "Unknown error"}")
        {
            Reason = reason;
        }

        public PaymentFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
