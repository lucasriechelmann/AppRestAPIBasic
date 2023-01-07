using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRestAPIBasic.Business.Models.Validations
{
    public class AddressValidation : AbstractValidator<Address>
    {
        public AddressValidation()
        {
            RuleFor(c => c.Street)
                .NotEmpty().WithMessage("The field {PropertyName} is required.")
                .Length(2, 200).WithMessage("The field {PropertyName} need to have between {MinLength} e {MaxLength} characteres");

            RuleFor(c => c.Neighborhood)
                .NotEmpty().WithMessage("The field {PropertyName} is required.")
                .Length(2, 100).WithMessage("The field {PropertyName} need to have between {MinLength} e {MaxLength} characteres");

            RuleFor(c => c.ZipCode)
                .NotEmpty().WithMessage("The field {PropertyName} is required.")
                .Length(8).WithMessage("The field {PropertyName} need to have {MaxLength} characteres");

            RuleFor(c => c.City)
                .NotEmpty().WithMessage("The field {PropertyName} is required")
                .Length(2, 100).WithMessage("The field {PropertyName} need to have between {MinLength} e {MaxLength} characteres");

            RuleFor(c => c.State)
                .NotEmpty().WithMessage("The field {PropertyName} is required.")
                .Length(2, 50).WithMessage("The field {PropertyName} need to have between {MinLength} e {MaxLength} characteres");

            RuleFor(c => c.Number)
                .NotEmpty().WithMessage("The field {PropertyName} is required.")
                .Length(1, 50).WithMessage("The field {PropertyName} need to have between {MinLength} e {MaxLength} characteres");
        }
    }
}
