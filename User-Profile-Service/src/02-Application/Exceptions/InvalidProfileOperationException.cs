namespace User_Profile_Service.src._02_Application.Exceptions
{
    public class InvalidProfileOperationException : Exception
    {
        public InvalidProfileOperationException(string message) : base(message) { }
        public InvalidProfileOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
