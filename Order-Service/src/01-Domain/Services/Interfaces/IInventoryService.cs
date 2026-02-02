namespace Order_Service.src._01_Domain.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<bool> CheckStockAsync(int productId, int quantity);
        Task<bool> ReserveStockAsync(int productId, int quantity);
        Task<bool> ReleaseStockAsync(int productId, int quantity);
    }
}
