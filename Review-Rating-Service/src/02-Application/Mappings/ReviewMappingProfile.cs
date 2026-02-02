using AutoMapper;
using Review_Rating_Service.src._01_Domain.Core.Aggregates.Review;
using Review_Rating_Service.src._02_Application.DTOs.Requests;
using Review_Rating_Service.src._02_Application.DTOs.Responses;

namespace Review_Rating_Service.src._02_Application.Mappings
{
    public class ReviewMappingProfile : Profile
    {
        public ReviewMappingProfile()
        {
            CreateMap<Review, ReviewResponseDto>()
                // اصلاح شده: گرفتن مقدار Value از آبجکت ReviewDate
                // چون ReviewDate در Domain یک کلاس (ValueObject) است
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => src.ReviewDate.Value))

                // نگاشت نام کاربر (از ValueObject ReviewerName)
                .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => src.ReviewerName.Value))

                // نگاشت متن (از پراپرتی Text که در Domain مستقیماً وجود دارد)
                // (اگر نام یکسان است، AutoMapper خودش انجام می‌دهد اما برای اطمینان)
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))

                // نگاشت پیوست‌ها
                .ForMember(dest => dest.AttachmentUrls, opt => opt.MapFrom(src =>
                    src.Attachments != null ? src.Attachments.Select(a => a.Url).ToList() : new List<string>()));

            CreateMap<Rating, RatingResponseDto>();

            CreateMap<CreateReviewRequestDto, Review>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}