using System.ComponentModel.DataAnnotations;

namespace App.Entities
{
    public class User
    {
        public int Id { get; set; }


        [Required, MaxLength(20)]
        public string NickName { get; set; }

        [Required, EmailAddress, MaxLength(30)]
        public string Email { get; set; }



        public string PasswordHash { get; set; }


        public virtual List<Review> Reviews { get; set; } = new List<Review>();


    }
}
