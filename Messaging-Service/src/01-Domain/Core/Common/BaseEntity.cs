using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messaging_Service.src._01_Domain.Core.Common
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; protected set; }

        // ✅ EF Core-friendly, no private backing field
        public DateTime CreatedDate { get; private set; }
        public DateTime? UpdatedDate { get; private set; }

        public bool IsDeleted { get; protected set; }

        protected BaseEntity()
        {
            CreatedDate = DateTime.UtcNow;
            IsDeleted = false;
        }

        // UnitOfWork methods
        public void SetCreationDate(DateTime date)
        {
            CreatedDate = date;
        }

        public void SetModificationDate(DateTime? date)
        {
            UpdatedDate = date;
        }

        public void SetAsDeleted()
        {
            IsDeleted = true;
            SetModificationDate(DateTime.UtcNow);
        }
    }
}
