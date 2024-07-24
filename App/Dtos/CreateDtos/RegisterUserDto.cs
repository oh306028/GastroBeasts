using System.ComponentModel.DataAnnotations;

namespace App.Dtos.CreateDtos
{
    public class RegisterUserDto
    {
        [Required, MaxLength(20)]
        public string NickName { get; set; }

        [Required, EmailAddress, MaxLength(30)]
        public string Email { get; set; }


        [Required, MinLength(5)]
        public string Password { get; set; }


        [Required, MinLength(5), Compare("Password")]
        public string ConfirmPasword { get; set; }  


    }
}
