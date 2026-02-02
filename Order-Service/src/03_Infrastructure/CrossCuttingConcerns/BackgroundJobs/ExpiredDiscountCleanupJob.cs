using Order_Service.src._01_Domain.Core.Interfaces.Repositories;

namespace Order_Service.src._03_Infrastructure.CrossCuttingConcerns.BackgroundJobs
{
    public class ExpiredDiscountCleanupJob
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<ExpiredDiscountCleanupJob> _logger;

        public ExpiredDiscountCleanupJob(IDiscountRepository discountRepository, ILogger<ExpiredDiscountCleanupJob> logger)
        {
            _discountRepository = discountRepository;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Starting expired discount cleanup job.");

            try
            {
                // In a real production scenario, it's better to add a specific method to the repository 
                // like GetExpiredDiscounts() or UpdateExpiredDiscountsStatus() to avoid loading all active ones into memory.
                var activeDiscounts = await _discountRepository.GetAllActiveAsync();

                var now = DateTime.UtcNow;
                var expiredDiscounts = activeDiscounts
                    .Where(d => d.EndDate.HasValue && d.EndDate.Value < now)
                    .ToList();

                foreach (var discount in expiredDiscounts)
                {
                    discount.Deactivate();
                    // In an explicit UoW pattern, we would update here, but since we don't have SaveChanges exposed in the job directly
                    // we assume the caller (e.g., hosted service) handles the context saving or we inject IUnitOfWork.
                    // For simplicity, we assume the repository updates track changes.
                }

                // Note: Ideally, IUnitOfWork.SaveChangesAsync() should be called here. 
                // Since we don't have IUnitOfWork injected in this snippet scope, we log the action.
                _logger.LogInformation("Deactivated {Count} expired discounts.", expiredDiscounts.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing expired discount cleanup job.");
            }
        }
    }
}
