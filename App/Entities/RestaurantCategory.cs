namespace App.Entities
{
    public class RestaurantCategory
    {
        public int RestaurantId { get; set; }
        public virtual  Restaurant Restaurant { get; set; }


        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }


    }
}
