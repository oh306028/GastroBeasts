using App.Authorization;
using App.Dtos.CreateDtos;
using App.Dtos.DisplayDtos;
using App.Dtos.UpdateDtos;
using App.Entities;
using App.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace App.Services
{
    public interface IReviewService
    {
        int CreateReviewToRestaurant(CreateReviewDto dto, int restaurantId);
        IEnumerable<ReviewDto> GetAllReviewsFromRestaurant(int restaurantId);
        void DeleteReview(int restaurantId, int reviewId);
        void UpdateReview(int restaurantId, int reviewId, UpdateReviewDto dto);
    }

    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IAuthorizationService _authorizationHandler;

        public ReviewService(AppDbContext dbContext, IMapper mapper, IUserContextService userContextService, IAuthorizationService authorizationHandler)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userContextService = userContextService;
            _authorizationHandler = authorizationHandler;
        }


        public void DeleteReview(int restaurantId, int reviewId)
        {
            
            var reviewToDelete = _dbContext.Reviews.Include(u => u.ReviewedBy).FirstOrDefault(r => r.Id == reviewId);

            var restaurant = CheckRestaurant(restaurantId);

            if (reviewToDelete is null)
                throw new NotFoundException("Review not found");


            var authorizartionResult = _authorizationHandler
                .AuthorizeAsync(_userContextService.User, reviewToDelete,
                new ResourceOperationRequirement(OperationType.Delete)).Result;


            if (!authorizartionResult.Succeeded)
                throw new ForbidException("Cannot delete not yours reviews");


            _dbContext.Reviews.Remove(reviewToDelete);
            _dbContext.SaveChanges();


        }

        public void UpdateReview(int restaurantId, int reviewId, UpdateReviewDto dto)
        {
            var reviewToUpdate = _dbContext.Reviews
                .Include(u => u.ReviewedBy)
                .Include(s => s.Stars)
                .FirstOrDefault(r => r.Id == reviewId);
                
            var restaurant = CheckRestaurant(restaurantId);

            if (reviewToUpdate is null)
                throw new NotFoundException("Review not found");


            var authorizartionResult = _authorizationHandler
                .AuthorizeAsync(_userContextService.User, reviewToUpdate,
                new ResourceOperationRequirement(OperationType.Update)).Result;


            if (!authorizartionResult.Succeeded)
                throw new ForbidException("Cannot update not yours reviews");


            reviewToUpdate.Comment = dto.Comment is null ? reviewToUpdate.Comment : dto.Comment;


            if (dto.Stars != null)
            {
                var newStars = _dbContext.Stars.FirstOrDefault(i => i.Id == dto.Stars);

                reviewToUpdate.Stars = newStars;
            }


            _dbContext.SaveChanges();

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
                UserId = (int)_userContextService.UserId

            };
          
            _dbContext.Reviews.Add(createdReview);
            _dbContext.SaveChanges();

            return createdReview.Id;

        }


        public IEnumerable<ReviewDto> GetAllReviewsFromRestaurant(int restaurantId)
        {
            var restaurant = _dbContext.Restaurants
                .AsNoTracking()
                .Include(r => r.Reviews)
                .ThenInclude(s => s.Stars)
                .Include(r => r.Reviews)
                .ThenInclude(rb => rb.ReviewedBy)
                .FirstOrDefault(i => i.Id == restaurantId); 

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var restaurantReviews = _mapper.Map<List<ReviewDto>>(restaurant.Reviews);

            return restaurantReviews;
        }
    }
}
