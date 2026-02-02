using Messaging_Service.src._01_Domain.Core.Common;

namespace Messaging_Service.src._01_Domain.Core.Entities
{
    public class MessageTemplate : BaseEntity
    {
        public string Name { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public bool IsActive { get; private set; }

        protected MessageTemplate() { }

        public MessageTemplate(string name, string subject, string body)
        {
            Id = Guid.NewGuid();
            Name = name;
            Subject = subject;
            Body = body;
            IsActive = true;
        }

        public void UpdateTemplate(string subject, string body)
        {
            Subject = subject;
            Body = body;
            // اصلاح شد: پاس دادن DateTime.UtcNow به عنوان پارامتر
            SetModificationDate(System.DateTime.UtcNow);
        }
    }
}
