using AutoMapper;
using Catalog_Service.src._01_Domain.Core.Entities;
using Catalog_Service.src._03_Endpoints.DTOs.Responses.Admin;

namespace Catalog_Service.src._03_Endpoints.Mappers
{
    public class CategoryTreeProfile : Profile
    {
        public CategoryTreeProfile()
        {
            CreateMap<Category, CategoryTreeResponse>()
                .ForMember(dest => dest.Slug,
                    opt => opt.MapFrom(src => src.Slug.Value))
                .ForMember(dest => dest.SubCategories,
                    opt => opt.MapFrom(src => src.SubCategories));
        }
    }

}
