using System.ComponentModel.DataAnnotations;

namespace App.Entities
{
    public class TopDish
    {
        public int Id { get; set; }

        [Required, MaxLength(15)]
        public string Name { get; set; }




        public int CategoryId { get; set; } 
        public virtual Category Category { get; set; }
    }
}
