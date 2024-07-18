using System.ComponentModel.DataAnnotations;

namespace App.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(25)]
        public string Name { get; set; }



        public int RestaurantId { get; set; }   
        public virtual Restaurant Restaurant{ get; set; }




        public virtual List<TopDish> TopDish { get; set; } = new List<TopDish>();
    }
}
