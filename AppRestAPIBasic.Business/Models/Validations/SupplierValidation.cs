using AppRestAPIBasic.Business.Models.Validations.Documents;
using FluentValidation;

namespace AppRestAPIBasic.Business.Models.Validations
{
    public class SupplierValidation : AbstractValidator<Supplier>
    {
        public SupplierValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The field {PropertyName} is required.")
                .Length(2, 100).WithMessage("The field {PropertyName} need to have bewteen {MinLenght} and {MaxLenght}.");

            When(x => x.SupplierType == SupplierType.SoloTrader, () =>
            {
                RuleFor(x => x.Document.Length)
                    .Equal(CPFValidation.CPF_LENGTH)
                    .WithMessage("The field Document need to have {ComparisonValue}.");
                RuleFor(x => CPFValidation.Validate(x.Document))
                    .Equal(true)
                    .WithMessage("The Document inputted is invalid");
            });

            When(x => x.SupplierType == SupplierType.LimitedCompany, () =>
            {
                RuleFor(x => x.Document.Length)
                    .Equal(CNPJValidation.CNPJ_LENGTH)
                    .WithMessage("The field Document need to have {ComparisonValue}.");
                RuleFor(x => CNPJValidation.Validate(x.Document))
                    .Equal(true)
                    .WithMessage("The Document inputted is invalid");
            });
        }
    }
}
