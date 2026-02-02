namespace User_Profile_Service.src._02_Application.Exceptions
{
    public class UserProfileNotFoundException : Exception
    {
        public UserProfileNotFoundException(string message) : base(message) { }
        public UserProfileNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
