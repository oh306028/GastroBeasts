using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Entities
{
    public class Stars
    {
        public int Id { get; set; }

        [Required, DefaultValue(3), Range(1,5)]
        public int Star { get; set; }


        [Required, MaxLength(15)]
        public string Rating { get; set; }  


        public virtual List<Review> Reviews { get; set; } = new List<Review>(); 
    }
}
