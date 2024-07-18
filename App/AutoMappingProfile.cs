using App.Dtos.CreateDtos;
using App.Dtos.DisplayDtos;
using App.Entities;
using AutoMapper;

namespace App
{
    public class AutoMappingProfile : Profile   
    {
        public AutoMappingProfile() 
        {
            CreateMap<CreateRestaurantDto, Restaurant>();

            CreateMap<Address, AddressDto>();

            CreateMap<User, UserDto>(); 

            CreateMap<Stars, StarsDto>();

            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.TopDishes, opt => opt.MapFrom(src => src.TopDish));

            CreateMap<TopDish, TopDishDto>();

            CreateMap<Restaurant, RestaurantDto>()
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                   .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.RestaurantCategories.Select(rc => rc.Category)))
                   .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews));


            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.Stars, opt => opt.MapFrom(src => src.Stars))
                .ForMember(dest => dest.ReviewedBy, opt => opt.MapFrom(src => src.ReviewedBy));

       


        }
    }
}
