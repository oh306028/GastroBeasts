using App.Dtos.CreateDtos;
using App.Entities;
using App.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Services
{
    public interface IReviewService
    {
        int CreateReviewToRestaurant(CreateReviewDto dto, int restaurantId);
    }

    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ReviewService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;    
        }

        private Restaurant CheckRestaurant(int restaurantId)
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(i => i.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            return restaurant;
        }

        public int CreateReviewToRestaurant(CreateReviewDto dto, int restaurantId)
        {
            var restaurant = CheckRestaurant(restaurantId);

  
            var createdReview = new Review()
            {
                Comment = dto.Comment,
                StarsId = dto.Stars,
                Restaurant = restaurant,
                UserId = 1
                
            };

            
            _dbContext.Reviews.Add(createdReview);
            _dbContext.SaveChanges();

            return createdReview.Id;

        }
    }
}
