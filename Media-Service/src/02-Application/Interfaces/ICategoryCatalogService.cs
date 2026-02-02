namespace Media_Service.src._02_Application.Interfaces
{
    public interface ICategoryCatalogService
    {
        Task<CategoryCatalogInfo> GetCategoryByIdAsync(Guid id);
    }

    public class CategoryCatalogInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? BrandId { get; set; } // برای دسترسی به برند والد
        public string? BrandName { get; set; } // برای دسترسی به نام برند در ساخت مسیر
        public Guid? ParentId { get; set; }
    }
}
