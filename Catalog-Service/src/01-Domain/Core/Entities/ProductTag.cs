namespace Catalog_Service.src._01_Domain.Core.Entities
{
    public class ProductTag : Entity
    {
        public int ProductId { get; private set; }
        public string TagText { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime CreatedAt { get; private set; }

        // Navigation properties
        public Product Product { get; private set; }

        // For EF Core
        protected ProductTag() { }

        public ProductTag(int productId, string tagText)
        {
            ProductId = productId;
            TagText = tagText;
            IsDeleted = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateTagText(string tagText)
        {
            TagText = tagText;
        }
    }
}