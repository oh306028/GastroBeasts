using App.Dtos.DisplayDtos;
using AutoMapper;

namespace App.Services
{
    public interface ICategoryService
    {
        IEnumerable<RestaurantCategoriesDto> GetAllRestaurantCategories();
    }
        
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;


        public CategoryService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public IEnumerable<RestaurantCategoriesDto> GetAllRestaurantCategories()
        {
            var categories = _dbContext.Categories.ToList();

            var mappedCategories = _mapper.Map<List<RestaurantCategoriesDto>>(categories);

            return mappedCategories;
        }



    }
}
