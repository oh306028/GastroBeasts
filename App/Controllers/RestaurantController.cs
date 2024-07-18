using App.Dtos;
using App.Entities;
using App.Services;
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
        public ActionResult<IEnumerable<RestaurantDto>> GetRestaurants()
        {
            var restaurants = _restaurantService.GetAllRestaurants();

            return Ok(restaurants);
        }


        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> GetRestaurantById([FromRoute]int id)
        {
            var restaurant = _restaurantService.GetRestaurantById(id);

            return Ok(restaurant);
        }




    }
}
