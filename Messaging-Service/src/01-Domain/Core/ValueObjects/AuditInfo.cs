using Messaging_Service.src._01_Domain.Core.Common;

namespace Messaging_Service.src._01_Domain.Core.ValueObjects
{
    public class AuditInfo : ValueObject
    {
        public string IpAddress { get; private set; }
        public string UserAgent { get; private set; }

        public AuditInfo(string ipAddress, string userAgent)
        {
            IpAddress = ipAddress;
            UserAgent = userAgent;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return IpAddress;
            yield return UserAgent;
        }
    }
}
