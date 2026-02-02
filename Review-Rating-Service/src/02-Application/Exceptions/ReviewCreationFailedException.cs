namespace Review_Rating_Service.src._02_Application.Exceptions
{
    public class ReviewCreationFailedException : Exception
    {
        public ReviewCreationFailedException(string message) : base(message) { }
        public ReviewCreationFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
