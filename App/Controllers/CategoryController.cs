using App.Dtos.CreateDtos;
using App.Dtos.DisplayDtos;
using App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{

    [Route("api/restaurants")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
           _categoryService = categoryService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("categories")]
        public ActionResult CreateNewCategory([FromBody]CreateNewCategoryDto dto)
        {
            _categoryService.CreateNewCategory(dto);    

            return Created("api/restaurants/categories", null);
        }

        

        [HttpGet("categories")]
        public ActionResult<IEnumerable<RestaurantCategoriesDto>> GetCategories()   
        {
            var categories = _categoryService.GetAllCategories();   

            return Ok(categories);
        }



        [HttpGet("{restaurantId}/categories")]
        public ActionResult<IEnumerable<RestaurantCategoriesDto>> GetRestaurantCategories([FromRoute] int restaurantId) 
        {
            var categories = _categoryService.GetAllRestaurantCategories(restaurantId);

            return Ok(categories);
        }
            



        [HttpPost("{restaurantId}/categories")]
        [Authorize]
        public ActionResult CreateCategoryForRestaurant([FromRoute] int restaurantId, [FromBody] CreateNewCategoryDto dto)   
        {
            _categoryService.AddCategoryToRestaurant(restaurantId, dto);

            return Created($"api/restaurant/{restaurantId}", null);
        }

    }
}
