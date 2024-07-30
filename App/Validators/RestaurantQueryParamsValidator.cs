using App.Dtos.CreateDtos;
using App.Dtos.QueryParams;
using FluentValidation;

namespace App.Validators
{
    public class RestaurantQueryParamsValidator : AbstractValidator<RestaurantQuery>
    {
        public RestaurantQueryParamsValidator()
        {
            RuleFor(ps => ps.PageSize).NotNull().GreaterThanOrEqualTo(1);

            RuleFor(pn => pn.PageNumber).NotNull().GreaterThanOrEqualTo(1);
        }
    }
}
