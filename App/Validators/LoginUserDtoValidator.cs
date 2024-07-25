using App.Dtos.CreateDtos;
using FluentValidation;

namespace App.Validators
{
    public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserDtoValidator()
        {
            RuleFor(e => e.Email)   
                .NotNull()
                .EmailAddress();

            RuleFor(p => p.Password)
                .NotNull()
                .MinimumLength(5);
        }
    }
}
