using System.ComponentModel.DataAnnotations;

namespace App.Dtos.CreateDtos
{
    public class CreateRestaurantDto
    {
        [Required, MaxLength(30)]
        public string Name { get; set; }  

        [Required, MaxLength(200)]
        public string Description { get; set; }       

            
    }
}
