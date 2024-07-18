using App.Entities;

namespace App.Dtos.DisplayDtos
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AddressDto Address { get; set; }
        public List<CategoryDto> Categories { get; set; }

        public List<Review> Reviews { get; set; }


    }
}
