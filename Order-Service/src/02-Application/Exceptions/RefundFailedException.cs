namespace Order_Service.src._02_Application.Exceptions
{
    public class RefundFailedException : Exception
    {
        public string? Reason { get; }

        public RefundFailedException(string? reason = null)
            : base($"Refund processing failed. Reason: {reason ?? "Unknown error"}")
        {
            Reason = reason;
        }

        public RefundFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
