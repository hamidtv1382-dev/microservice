namespace Order_Service.src._01_Domain.Services.Interfaces
{
    public interface IJwtTokenValidationService
    {
        bool ValidateToken(string token);
        Guid? GetUserIdFromToken(string token);
    }
}
