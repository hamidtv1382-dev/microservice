using AutoMapper;
using Catalog_Service.src._01_Domain.Core.Entities;
using Catalog_Service.src._03_Endpoints.DTOs.Responses.Admin;

namespace Catalog_Service.src._03_Endpoints.Mappers
{
    public class AdminCategoryProfile : Profile
    {
        public AdminCategoryProfile()
        {
            CreateMap<Category, AdminCategoryResponse>()
                .ForMember(dest => dest.Slug,
                    opt => opt.MapFrom(src => src.Slug.Value))

                .ForMember(dest => dest.ParentCategoryName,
                    opt => opt.MapFrom(src =>
                        src.ParentCategory != null ? src.ParentCategory.Name : null))

                .ForMember(dest => dest.ProductCount,
                    opt => opt.MapFrom(src => src.Products.Count))

                .ForMember(dest => dest.SubCategoryCount,
                    opt => opt.MapFrom(src => src.SubCategories.Count))

                .ForMember(dest => dest.SubCategories,
                    opt => opt.MapFrom(src => src.SubCategories));


            CreateMap<Category, CategoryResponse>()
                .ForMember(dest => dest.Slug,
                    opt => opt.MapFrom(src => src.Slug.Value));
        }
    }
}
