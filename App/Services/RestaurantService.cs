using App.Dtos;
using App.Entities;
using App.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Services
{
    public interface IRestaurantService
    {
        IEnumerable<RestaurantDto> GetAllRestaurants();
        RestaurantDto GetRestaurantById(int id);
    }   

    public class RestaurantService : IRestaurantService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public RestaurantService(AppDbContext dbContext, IMapper mapper)    
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public IEnumerable<RestaurantDto> GetAllRestaurants()
        {
            var restaurants = _dbContext.Restaurants
                .Include(a => a.Address)
                .Include(rc => rc.RestaurantCategories)
                .ThenInclude(c => c.Category)
                .Include(rv => rv.Reviews);

            return _mapper.Map<List<RestaurantDto>>(restaurants);   
        }


        public RestaurantDto GetRestaurantById(int id)
        {
            var restaurant = _dbContext.Restaurants.Include(a => a.Address)
                .Include(rc => rc.RestaurantCategories)
                .ThenInclude(c => c.Category)
                .Include(rv => rv.Reviews)
                .FirstOrDefault(i => i.Id == id);



            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");


            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            return restaurantDto;

        }
    }
}
