using App.Authorization;
using App.Dtos.CreateDtos;
using App.Dtos.DisplayDtos;
using App.Dtos.QueryParams;
using App.Dtos.UpdateDtos;
using App.Entities;
using App.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace App.Services
{
    //TO DO:
    //filtering by category name
    public interface IRestaurantService
    {
        RestaurantDto GetRestaurantById(int id, bool includeReviews);
        int CreateRestaurant(CreateRestaurantDto dto);
        void DeleteRestaurant(int restaurantId);
        void UpdateRestaurant(int restaurantId, UpdateRestaurantDto dto);

        IEnumerable<RestaurantDto> GetAllRestaurants(RestaurantQuery queryParams);
    }   

    public class RestaurantService : IRestaurantService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public RestaurantService(AppDbContext dbContext, IMapper mapper, IAuthorizationService authorizationService, IUserContextService userContextService)    
        {
            _dbContext = dbContext;
            _mapper = mapper;   
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public int CreateRestaurant(CreateRestaurantDto dto)
        {
            var createdRestaurant = _mapper.Map<Restaurant>(dto);


            createdRestaurant.UserId = (int)_userContextService.UserId;

            _dbContext.Restaurants.Add(createdRestaurant);
            _dbContext.SaveChanges();

            return createdRestaurant.Id;
        }


        public void DeleteRestaurant(int restaurantId)
        {
            var restaurantToDelete = _dbContext.Restaurants.FirstOrDefault(i => i.Id == restaurantId);

            if (restaurantToDelete is null)
                throw new NotFoundException("Restaurant not found");


            var authorizartionResult =  _authorizationService  
                   .AuthorizeAsync(_userContextService.User, restaurantToDelete, new ResourceOperationRequirement(OperationType.Delete)).Result;


            if (!authorizartionResult.Succeeded)
                throw new ForbidException("Cannot delete not yours restaurants");

            _dbContext.Remove(restaurantToDelete);
            _dbContext.SaveChanges();

        }

        public void UpdateRestaurant(int restaurantId, UpdateRestaurantDto dto)
        {
            var restaurantToUpdate = _dbContext.Restaurants.FirstOrDefault(i => i.Id == restaurantId);

            if (restaurantToUpdate is null)
                throw new NotFoundException("Restaurant not found");

            var authenticationResult = _authorizationService
                .AuthorizeAsync(_userContextService.User, restaurantToUpdate, 
                new ResourceOperationRequirement(OperationType.Update)).Result;


            if (!authenticationResult.Succeeded)
                throw new ForbidException("Cannot update not yours restaurants");


            restaurantToUpdate.Name = dto.Name is null ? restaurantToUpdate.Name : dto.Name;
            restaurantToUpdate.Description = dto.Description is null ? restaurantToUpdate.Description : dto.Description;
                
            _dbContext.SaveChanges();

        }


        public IEnumerable<RestaurantDto> GetAllRestaurants(RestaurantQuery queryParams)    
        {
            IQueryable<Restaurant> query = _dbContext.Restaurants
                .AsNoTracking()
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
               Where(param => queryParams.RestaurantName == null || param.Name.ToLower().
               Contains(queryParams.RestaurantName.ToLower()))
               .ToList();

            
            

            return _mapper.Map<List<RestaurantDto>>(restaurants);

        }

        public RestaurantDto GetRestaurantById(int id, bool includeReviews)
        {
            IQueryable<Restaurant> query = _dbContext
                .Restaurants
                .AsNoTracking()
                .Include(a => a.Address)
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
