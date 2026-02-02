namespace Media_Service.src._02_Application.Exceptions
{
    public class MediaOwnerNotFoundException : Exception
    {
        public MediaOwnerNotFoundException(string message) : base(message) { }
        public MediaOwnerNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
