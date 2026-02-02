namespace Finance_Service.src._02_Application.Exceptions
{
    public class SettlementFailedException : Exception
    {
        public SettlementFailedException(string message) : base(message) { }
        public SettlementFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
