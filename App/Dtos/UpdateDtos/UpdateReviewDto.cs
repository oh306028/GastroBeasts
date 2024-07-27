using System.ComponentModel.DataAnnotations;

namespace App.Dtos.UpdateDtos
{
    public class UpdateReviewDto
    {
        [MaxLength(100)]
        public string? Comment { get; set; }

        [Range(1,5)]
        public int? Stars { get; set; }  
    }
}
