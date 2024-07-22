using System.ComponentModel.DataAnnotations;

namespace App.Entities
{
    public class Restaurant
    {

        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string Name { get; set; }

        [Required, MaxLength(200)]
        public string Description { get; set; } 


            
        public virtual Address Address { get; set; }

        public virtual List<RestaurantCategory> RestaurantCategories { get; set; } = new List<RestaurantCategory>();

        public virtual List<Review> Reviews { get; set; } = new List<Review>();



    }
}
