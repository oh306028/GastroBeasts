using System.ComponentModel.DataAnnotations;

namespace App.Dtos.CreateDtos
{
    public class CreateNewCategoryDto
    {
        [Required, MaxLength(25)]
        public string Name { get; set; }    
    }
}
