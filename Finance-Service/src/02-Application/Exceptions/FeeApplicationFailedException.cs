namespace Finance_Service.src._02_Application.Exceptions
{
    public class FeeApplicationFailedException : Exception
    {
        public FeeApplicationFailedException(string message) : base(message) { }
        public FeeApplicationFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
