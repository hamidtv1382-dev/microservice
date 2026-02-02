using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seller_Finance_Service.src._01_Domain.Core.Common
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; protected set; }

        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; protected set; }

        public bool IsDeleted { get; protected set; } = false;

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
