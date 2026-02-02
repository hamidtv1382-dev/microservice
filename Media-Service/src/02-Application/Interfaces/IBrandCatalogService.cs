namespace Media_Service.src._02_Application.Interfaces
{
    public interface IBrandCatalogService
    {
        Task<BrandCatalogInfo> GetBrandByIdAsync(Guid id);
    }

    public class BrandCatalogInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
