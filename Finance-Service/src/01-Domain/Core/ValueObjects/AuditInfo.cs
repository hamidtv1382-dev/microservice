using Finance_Service.src._01_Domain.Core.Common;

namespace Finance_Service.src._01_Domain.Core.ValueObjects
{
    public class AuditInfo : ValueObject
    {
        public string CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string? LastModifiedBy { get; private set; }
        public DateTime? LastModifiedAt { get; private set; }

        public AuditInfo(string createdBy)
        {
            if (string.IsNullOrWhiteSpace(createdBy))
                throw new ArgumentException("Creator cannot be empty", nameof(createdBy));

            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateAudit(string modifiedBy)
        {
            if (string.IsNullOrWhiteSpace(modifiedBy))
                throw new ArgumentException("Modifier cannot be empty", nameof(modifiedBy));

            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
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
