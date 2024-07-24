using App.Dtos.CreateDtos;
using FluentValidation;

namespace App.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>  
    {
        public RegisterUserDtoValidator(AppDbContext dbContext)
        {
          
            RuleFor(x => x.ConfirmPassword)
                .Equal(cp => cp.Password)
                .NotNull(); 

            RuleFor(x => x.NickName)
                .NotNull()
                .MaximumLength(20);

            RuleFor(x => x.Email)
                .NotNull()
                .EmailAddress();

            RuleFor(p => p.Password)
                .NotNull();

            RuleFor(e => e.Email)
                .Custom((value, context) => {
                    var emailInUse = dbContext.Users.Any(e => e.Email == value);   
                        
                    if(emailInUse)
                    {
                        context.AddFailure("Email", "Email is already taken");
                    }

            });


            RuleFor(n => n.NickName)
                .Custom((value, context) =>
                {

                    var nickInUse = dbContext.Users.Any(n => n.NickName == value);

                    if (nickInUse)
                    {
                        context.AddFailure("NickName", "Name is already taken");
                    }

                });
           
        }
    }
}
