using AutoMapper;
using CommunicationService.Database;
using CommunicationService.Models;

namespace CommunicationService.MappingConfig
{
    public class ProfileMapping : Profile
    {
        public ProfileMapping()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<CategoryPublishDto, CategoryDto>().ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.id));
        }
    }
}
