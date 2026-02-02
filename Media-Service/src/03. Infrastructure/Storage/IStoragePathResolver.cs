namespace Media_Service.src._03._Infrastructure.Storage
{
    public interface IStoragePathResolver
    {
        Task<string> GetRootPathAsync();
    }
}
