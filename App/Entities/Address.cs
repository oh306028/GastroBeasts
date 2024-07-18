using System.ComponentModel.DataAnnotations;

namespace App.Entities
{
    public class Address
    {
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string City { get; set; }

        [Required, MaxLength(25)]
        public string Street { get; set; }

        [Required, MaxLength(10)]
        public string Number { get; set; }


        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }




    }
}
