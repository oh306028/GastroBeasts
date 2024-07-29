using App.Dtos.CreateDtos;
using App.Dtos.DisplayDtos;
using App.Entities;
using App.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Services
{
    public interface ICategoryService
    {
        IEnumerable<RestaurantCategoriesDto> GetAllCategories();
        void CreateNewCategory(CreateNewCategoryDto dto);
        void AddCategoryToRestaurant(int restaurantId, CreateNewCategoryDto categoryName);
        IEnumerable<RestaurantCategoriesDto> GetAllRestaurantCategories(int restaurantId);
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


        public IEnumerable<RestaurantCategoriesDto> GetAllCategories()
        {
            var categories = _dbContext.Categories.ToList();

            var mappedCategories = _mapper.Map<List<RestaurantCategoriesDto>>(categories);

            return mappedCategories;
        }



        //TO DO:
        //query below takes only the last ids
        //refactor it to take all the categories
        public IEnumerable<RestaurantCategoriesDto> GetAllRestaurantCategories(int restaurantId)    
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(i => i.Id == restaurantId);

            if(restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var currentRestaurantcategories = _dbContext.RestaurantCategories
                .AsNoTracking()
                .Include(c => c.Category)
                .Where(r => r.RestaurantId == restaurantId)
                .ToList();

            var categoryIds = currentRestaurantcategories
                .Select(c => c.Category);
           
            var mappedCategories = _mapper.Map<List<RestaurantCategoriesDto>>(categoryIds);

            return mappedCategories;
        }





        public void CreateNewCategory(CreateNewCategoryDto dto)
        {        
            var existingCategory = _dbContext.Categories.FirstOrDefault(n => n.Name.ToLower() == dto.Name.ToLower());   

            if(existingCategory is null)
            {
                var mappedNewCategory = _mapper.Map<Category>(dto);

                mappedNewCategory.Name = char.ToUpper(mappedNewCategory.Name[0]) + mappedNewCategory.Name.Substring(1);

                _dbContext.Categories.Add(mappedNewCategory);
                _dbContext.SaveChanges();

                return;
            }

            throw new ExistingResourceException("That category already exists");
           
        }


        public void AddCategoryToRestaurant(int restaurantId, CreateNewCategoryDto dto)    
        {
            var restaurant =_dbContext.Restaurants.FirstOrDefault(i => i.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var category = _dbContext.Categories.FirstOrDefault(n => n.Name.ToLower() == dto.Name.ToLower());   

            if (category is null)
                throw new NotFoundException("Category does not exist");
             

            var currentRestaurant = _dbContext.Restaurants.Include(rc => rc.RestaurantCategories).FirstOrDefault(r => r.Id == restaurantId);   

            foreach(var cat in currentRestaurant.RestaurantCategories)
            {
                if (cat.Category.Name.ToLower() == dto.Name.ToLower())
                    throw new ExistingResourceException("That category already describe that restaurant");
            }
            

            var restaurantNewCategory = new RestaurantCategory()
            {
                Restaurant = restaurant,
                Category = category
            };

            _dbContext.RestaurantCategories.Add(restaurantNewCategory);
            _dbContext.SaveChanges();
    
        }


    }
}
