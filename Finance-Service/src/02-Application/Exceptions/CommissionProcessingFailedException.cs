namespace Finance_Service.src._02_Application.Exceptions
{
    public class CommissionProcessingFailedException : Exception
    {
        public CommissionProcessingFailedException(string message) : base(message) { }
        public CommissionProcessingFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
