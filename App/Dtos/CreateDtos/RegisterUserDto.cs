using System.ComponentModel.DataAnnotations;

namespace App.Dtos.CreateDtos
{
    public class RegisterUserDto
    {
        [Required, MaxLength(20)]
        public string NickName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }


        [Required, MinLength(5)]
        public string Password { get; set; }


        [Required, MinLength(5)]
        public string ConfirmPassword { get; set; }  


    }
}
