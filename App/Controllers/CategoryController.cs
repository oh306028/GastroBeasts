using App.Dtos.DisplayDtos;
using App.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{

    [Route("api/restaurants/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
           _categoryService = categoryService;
        }



        [HttpGet]
        public ActionResult<IEnumerable<RestaurantCategoriesDto>> GetRestaurantCategories()
        {
            var categories = _categoryService.GetAllRestaurantCategories();

            return Ok(categories);
        }


    }
}
