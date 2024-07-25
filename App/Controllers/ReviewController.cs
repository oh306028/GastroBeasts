using App.Dtos.CreateDtos;
using App.Dtos.DisplayDtos;
using App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/restaurants/{restaurantId}/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }


        [HttpPost]
        [Authorize]
        public ActionResult<int> CreateReview([FromBody] CreateReviewDto dto, [FromRoute] int restaurantId)
        {
            int createdReviewId = _reviewService.CreateReviewToRestaurant(dto, restaurantId);

            return Created($"api/restaurants/{restaurantId}/reviews/{createdReviewId}", null);

        }


        [HttpGet]
        public ActionResult<IEnumerable<ReviewDto>> GetReviews([FromRoute]int restaurantId)
        {
            var reviews = _reviewService.GetAllReviewsFromRestaurant(restaurantId);

            return Ok(reviews);
        }

    }
}
