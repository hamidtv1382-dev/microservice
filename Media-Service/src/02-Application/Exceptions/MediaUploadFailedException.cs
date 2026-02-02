namespace Media_Service.src._02_Application.Exceptions
{
    public class MediaUploadFailedException : Exception
    {
        public MediaUploadFailedException(string message) : base(message) { }
        public MediaUploadFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
