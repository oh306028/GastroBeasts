using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Entities
{
    public class Stars
    {
        public int Id { get; set; }

        [Required, DefaultValue(3), Range(1,5)]
        public int Star { get; set; }



        public int ReviewId { get; set; }   
        public virtual Review Review { get; set; } 
    }
}
