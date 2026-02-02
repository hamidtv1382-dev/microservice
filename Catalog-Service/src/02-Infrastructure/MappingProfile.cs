using AutoMapper;
using Catalog_Service.src._01_Domain.Core.Entities;
using Catalog_Service.src._01_Domain.Core.Primitives;
using Catalog_Service.src._03_Endpoints.DTOs.Responses.Vendor;
using System.Linq;

namespace Catalog_Service.src._02_Infrastructure
{
    // Resolver سفارشی برای پیدا کردن آدرس تصویر اصلی
    public class PrimaryImageUrlResolver : IValueResolver<Product, VendorProductResponse, string>
    {
        public string Resolve(Product source, VendorProductResponse destination, string destMember, ResolutionContext context)
        {
            var primaryImage = source.Images.FirstOrDefault(i => i.IsPrimary);
            return primaryImage != null ? primaryImage.PublicUrl : null;
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 1. مپینگ Product به VendorProductResponse
            CreateMap<Product, VendorProductResponse>()
                // مپ کردن قیمت اصلی (Money -> decimal)
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))

                // مپ کردن قیمت قبلی (Nullable<Money> -> decimal?)
                .ForMember(dest => dest.OriginalPrice, opt => opt.MapFrom(src => src.OriginalPrice != null ? src.OriginalPrice.Amount : (decimal?)null))

                // مپ کردن Slug (Slug -> string)
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug.Value))

                // پیدا کردن آدرس تصویر اصلی با Resolver
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<PrimaryImageUrlResolver>())

                // مپ کردن ابعاد و وزن (Value Objects)
                .ForMember(dest => dest.Dimensions, opt => opt.MapFrom(src => src.Dimensions))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight));

            // 2. مپینگ Value Objects داخلی
            CreateMap<Dimensions, VendorDimensionsResponse>();
            CreateMap<Weight, VendorWeightResponse>();

            // 3. مپینگ ImageResource
            CreateMap<ImageResource, VendorProductImageResponse>();

            // 4. مپینگ سایر موجودیت‌های وابسته (برای لیست‌ها)
            CreateMap<ProductVariant, VendorProductVariantResponse>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.OriginalPrice, opt => opt.MapFrom(src => src.OriginalPrice != null ? src.OriginalPrice.Amount : (decimal?)null))
                .ForMember(dest => dest.Dimensions, opt => opt.MapFrom(src => src.Dimensions))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight));

            CreateMap<ProductAttribute, VendorProductAttributeResponse>();
        }
    }
}
