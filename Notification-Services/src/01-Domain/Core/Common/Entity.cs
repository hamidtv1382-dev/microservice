namespace Notification_Services.src._01_Domain.Core.Common
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public bool IsDeleted { get; protected set; }

        protected Entity()
        {
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }

        public void SetUpdatedAt()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
            SetUpdatedAt();
        }
    }
}
