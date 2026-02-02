namespace Finance_Service.src._02_Application.Interfaces
{
    public interface IOrderServiceClient
    {
        Task<bool> ConfirmOrderAsync(Guid orderId);
    }
}
