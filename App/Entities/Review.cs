using System.ComponentModel.DataAnnotations;

namespace App.Entities
{
    public class Review
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string? Comment { get; set; }

        [Required]
        public DateTime PostTime { get; set; }  



        [Required]  
        public virtual Stars Stars { get; set; }
        public int StarsId { get; set; }    


        public int RestaurantId { get; set; }   
        public virtual Restaurant Restaurant { get; set; }



        public virtual User ReviewedBy { get; set; }    
        public int UserId { get; set; } 



    }
}
