namespace Review_Rating_Service.src._01_Domain.Services.Interfaces
{
    public interface IReviewDomainService
    {
        Task<bool> CanUserReviewAsync(Guid userId, int productId);
    }
}
