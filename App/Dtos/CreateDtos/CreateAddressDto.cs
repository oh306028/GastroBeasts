using System.ComponentModel.DataAnnotations;

namespace App.Dtos.CreateDtos
{
    public class CreateAddressDto
    {
        [Required, MaxLength(20)]
        public string City { get; set; }

        [Required, MaxLength(25)]
        public string Street { get; set; }

        [Required, MaxLength(10)]
        public string Number { get; set; }      
    }
}
