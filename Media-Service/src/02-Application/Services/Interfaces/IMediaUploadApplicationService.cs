using Media_Service.src._02_Application.DTOs.Requests;
using Media_Service.src._02_Application.DTOs.Responses;

namespace Media_Service.src._02_Application.Services.Interfaces
{
    public interface IMediaUploadApplicationService
    {
        Task<MediaUploadResponseDto> UploadBrandMediaAsync(UploadBrandMediaRequestDto request);
        Task<MediaUploadResponseDto> UploadCategoryMediaAsync(UploadCategoryMediaRequestDto request);
        Task<MediaUploadResponseDto> UploadProductMediaAsync(UploadProductMediaRequestDto request);
    }
}
