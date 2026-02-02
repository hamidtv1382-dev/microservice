namespace Notification_Services.src._02_Application.Exceptions
{
    public class TemplateNotFoundException : Exception
    {
        public TemplateNotFoundException(string message) : base(message) { }
        public TemplateNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
