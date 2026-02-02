using Messaging_Service.src._01_Domain.Core.Common;

namespace Messaging_Service.src._01_Domain.Core.ValueObjects
{
    public class MessageContent : ValueObject
    {
        public string Text { get; private set; }

        public MessageContent(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Message content cannot be empty.");

            Text = text;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Text;
        }
    }
}
