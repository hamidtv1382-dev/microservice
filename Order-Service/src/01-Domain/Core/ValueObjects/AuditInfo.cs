using Order_Service.src._01_Domain.Core.Common;

namespace Order_Service.src._01_Domain.Core.ValueObjects
{
    public class AuditInfo : ValueObject
    {
        public string CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string? LastModifiedBy { get; private set; }
        public DateTime? LastModifiedAt { get; private set; }

        protected AuditInfo() { }

        public AuditInfo(string createdBy, DateTime createdAt, string? lastModifiedBy, DateTime? lastModifiedAt)
        {
            if (string.IsNullOrWhiteSpace(createdBy))
                throw new ArgumentException("CreatedBy cannot be empty.", nameof(createdBy));

            CreatedBy = createdBy.Trim();
            CreatedAt = createdAt;
            LastModifiedBy = lastModifiedBy?.Trim();
            LastModifiedAt = lastModifiedAt;
        }

        public static AuditInfo Create(string userId)
        {
            return new AuditInfo(userId, DateTime.UtcNow, null, null);
        }

        public AuditInfo Update(string userId)
        {
            return new AuditInfo(CreatedBy, CreatedAt, userId, DateTime.UtcNow);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CreatedBy;
            yield return CreatedAt;
            yield return LastModifiedBy;
            yield return LastModifiedAt;
        }
    }
}
