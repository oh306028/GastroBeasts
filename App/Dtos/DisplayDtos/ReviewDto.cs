namespace App.Dtos.DisplayDtos
{
    public class ReviewDto
    {
        public string Comment { get; set; }
        public DateTime PostTime { get; set; }

        public StarsDto Stars { get; set; }

        public UserDto ReviewedBy { get; set; }
    }
}
