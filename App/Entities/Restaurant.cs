using System.ComponentModel.DataAnnotations;

namespace App.Entities
{
    public class Restaurant
    {

        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string Description { get; set; } 



        public virtual Address Address { get; set; }

        public virtual List<Category> Categories { get; set; } = new List<Category>();

        public virtual List<Review> Reviews { get; set; } = new List<Review>();



    }
}
