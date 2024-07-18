using System.ComponentModel.DataAnnotations;

namespace App.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(25)]
        public string Name { get; set; }


        public virtual List<RestaurantCategory> RestaurantCategories { get; set; } = new List<RestaurantCategory>();


        public virtual List<TopDish> TopDish { get; set; } = new List<TopDish>();
    }
}
