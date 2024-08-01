using App.Dtos.CreateDtos;
using App.Dtos.DisplayDtos;
using App.Dtos.QueryParams;
using App.Dtos.Results;
using App.Dtos.UpdateDtos;
using App.Entities;
using App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/restaurants")]
    [ApiController]
    public class RestaurantController : ControllerBase  
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public ActionResult<PagedResults<RestaurantDto>> GetRestaurants([FromQuery]RestaurantQuery queryParams)  
        {
            var restaurants = _restaurantService.GetAllRestaurants(queryParams);

            return Ok(restaurants);
        }


        [HttpGet("all")]
        public ActionResult<IEnumerable<RestaurantDto>> GetAllRestaurants()
        {
            var restaurants = _restaurantService.GetRestaurantsNoPagination();

            return Ok(restaurants);
        }


        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> GetRestaurantById([FromRoute]int id, [FromQuery] bool includeReviews)    
        {
            var restaurant = _restaurantService.GetRestaurantById(id, includeReviews);

            return Ok(restaurant);
        }


        [HttpPost]
        [Authorize]
        public ActionResult<int> CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            int createdRestaurantId = _restaurantService.CreateRestaurant(dto);

            return Created($"api/restaurants/{createdRestaurantId}", null);

        }


        [HttpDelete("{restaurantId}")]       
        [Authorize]
        public ActionResult DeleteRestaurant([FromRoute]int restaurantId)
        {
            _restaurantService.DeleteRestaurant(restaurantId);

            return NoContent();
        }



        [HttpPut("{restaurantId}")] 
        [Authorize]
        public ActionResult UpdateRestaurant([FromRoute] int restaurantId, [FromBody] UpdateRestaurantDto dto)
        {
            _restaurantService.UpdateRestaurant(restaurantId, dto);

            return NoContent();
        }



    }
}
