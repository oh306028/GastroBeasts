using App.Dtos.DisplayDtos;
using System.ComponentModel.DataAnnotations;

namespace App.Dtos.CreateDtos
{
    public class CreateReviewDto
    {
        [MaxLength(100)]
        public string Comment { get; set; }

        [Required, Range(1,5)]
        public int Stars { get; set; }

        public int RestaurantId { get; set; }   
    }
}
