using App.Dtos.CreateDtos;
using App.Dtos.DisplayDtos;
using App.Dtos.QueryParams;
using App.Entities;
using App.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace App.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetRestaurantById(int id, bool includeReviews);
        int CreateRestaurant(CreateRestaurantDto dto);

        IEnumerable<RestaurantDto> GetAllRestaurants(RestaurantQuery queryParams);
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

        public int CreateRestaurant(CreateRestaurantDto dto)
        {
            var createdRestaurant = _mapper.Map<Restaurant>(dto);

            _dbContext.Restaurants.Add(createdRestaurant);
            _dbContext.SaveChanges();

            return createdRestaurant.Id;
        }

     
        public IEnumerable<RestaurantDto> GetAllRestaurants(RestaurantQuery queryParams)    
        {
            IQueryable<Restaurant> query = _dbContext.Restaurants
              .Include(a => a.Address)
              .Include(rc => rc.RestaurantCategories)
              .ThenInclude(c => c.Category);

            if (queryParams.IncludeReviews != null && queryParams.IncludeReviews == true)
            {
                query = query
                    .Include(rv => rv.Reviews)
                        .ThenInclude(u => u.Stars)
                    .Include(rv => rv.Reviews)
                        .ThenInclude(u => u.ReviewedBy);
            }


            var restaurants = query.ToList();


                restaurants = restaurants.
               Where(param =>param.Name.ToLower().
               Contains(queryParams.RestaurantName.ToLower())
               || queryParams.RestaurantName == null)
               .ToList();
            


            return _mapper.Map<List<RestaurantDto>>(restaurants);

        }

        public RestaurantDto GetRestaurantById(int id, bool includeReviews)
        {
            IQueryable<Restaurant> query = _dbContext.Restaurants.Include(a => a.Address)
                .Include(rc => rc.RestaurantCategories)
                .ThenInclude(c => c.Category);


            if (includeReviews)
            {
                query = query.
                    Include(r => r.Reviews)
                    .ThenInclude(s => s.Stars)
                    .Include(r => r.Reviews).
                    ThenInclude(rb => rb.ReviewedBy);
            }

            var restaurants = query.ToList();
            var restaurant = restaurants.FirstOrDefault(i => i.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");


            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            return restaurantDto;

        }
    }
}
