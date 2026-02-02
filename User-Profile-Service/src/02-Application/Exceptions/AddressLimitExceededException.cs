namespace User_Profile_Service.src._02_Application.Exceptions
{
    public class AddressLimitExceededException : Exception
    {
        public AddressLimitExceededException(string message) : base(message) { }
        public AddressLimitExceededException(string message, Exception innerException) : base(message, innerException) { }
    }
}
