using FluentValidation;

namespace AppRestAPIBasic.Business.Models.Validations
{
    public class ProductValidation : AbstractValidator<Product>
    {
        public ProductValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("The field {PropertyName} is required.")
                .Length(2, 200).WithMessage("The field {PropertyName} need to have between {MinLength} and {MaxLength} characters");

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("The field {PropertyName} is required.")
                .Length(2, 1000).WithMessage("The field {PropertyName} need to have between {MinLength} and {MaxLength} characters");

            RuleFor(c => c.Value)
                .GreaterThan(0).WithMessage("The field {PropertyName} need to be greater than {ComparisonValue}");
        }
    }
}
