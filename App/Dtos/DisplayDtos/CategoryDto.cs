namespace App.Dtos.DisplayDtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<TopDishDto> TopDishes { get; set; }
    }
}
