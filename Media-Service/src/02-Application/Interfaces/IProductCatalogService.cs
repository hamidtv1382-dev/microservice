namespace Media_Service.src._02_Application.Interfaces
{
    public interface IProductCatalogService
    {
        Task<ProductCatalogInfo> GetProductByIdAsync(Guid id);
    }

    public class ProductCatalogInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
