using App.Entities;

namespace App
{
    public interface IApplicationSeeder
    {
        Task SeedAsync();   
    }

    public class ApplicationSeeder : IApplicationSeeder
    {
        private readonly AppDbContext _context;

        public ApplicationSeeder(AppDbContext context)
        {
            _context = context;
        }


        public async Task SeedAsync()   
        {
            

            if (!_context.Restaurants.Any())
            {
                var restaurants = getRestaurants();

                await _context.Restaurants.AddRangeAsync(restaurants);
                await _context.SaveChangesAsync();

                var restaurantCategories = getRestaurantCategories();

                await _context.RestaurantCategories.AddRangeAsync(restaurantCategories);
                await _context.SaveChangesAsync();

            }

        }

        private IEnumerable<Restaurant> getRestaurants()
        {
            IEnumerable<Restaurant> restaurants = new List<Restaurant>()
            {

                new Restaurant()
                {
                    Name = "Kebab u Alika",
                    Description = "Podstawą naszego kebaba jest dobrej jakości mięso, produkowane na nasze zamówienie.To my decydujemy jaki jest jego skład.W ofercie mamy również mięso wołowo-baranie PIRI PIRI.",

                    Address = new Address()
                    {
                        City = "Mysłowice",
                        Street = "Chopina",
                        Number = "21F"
                    },


                },

                new Restaurant()
                {
                    Name = "Nova Sushi",
                    Description = "Tu ważny jest każdy etap, dlatego sushi-masterzy w NOVASUSHI robią wszystko od podstaw. Codziennie sami ręcznie filetujemy ryby, przygotowujemy ryż, warzywa oraz sosy.",
                    Address = new Address()
                    {
                        City = "Katowice",
                        Street = "Szewska",
                        Number = "12"
                    }


                },

                new Restaurant()
                {
                    Name = "Pasibus",
                    Description = "Nawet gorol (nie-Ślązak) wie, że być w Katowicach i nie wpaść do Pasibusa to je gańba (czyli wstyd). Zwłaszcza, że nasze najlepsze burgery toczą bój o lokalne podniebienia od 2018 roku",
                    Address = new Address()
                    {
                        City = "Katowice",
                        Street = "Chorzowska",
                        Number = "107"
                    }
                }
            };

            return restaurants;

        }

        private IEnumerable<RestaurantCategory> getRestaurantCategories()   
        {

           int kebabId = _context.Restaurants.First(n => n.Name == "Kebab u Alika").Id;
           int sushiId = _context.Restaurants.First(n => n.Name == "Nova Sushi").Id;
           int pasiId = _context.Restaurants.First(n => n.Name == "Pasibus").Id;  

            IEnumerable<RestaurantCategory> categories = new List<RestaurantCategory>()
            {
              
                new RestaurantCategory()
                {

                    CategoryId = 1,
                    RestaurantId = kebabId
                },


                 new RestaurantCategory()
                {
                    CategoryId = 1,
                    RestaurantId = pasiId,
                },

                    new RestaurantCategory()
                {
                    CategoryId = 3,
                    RestaurantId = pasiId,
                },

                new RestaurantCategory()
                {
                    RestaurantId = sushiId,
                    CategoryId = 3
                }

            };

            return categories;
        }


    }
}
