using AppRestAPIBasic.Business.Interfaces;
using AppRestAPIBasic.Business.Models;
using FluentValidation;
using FluentValidation.Results;

namespace AppRestAPIBasic.Business.Services
{
    public abstract class BaseService
    {
        readonly INotifier _notifier;

        protected BaseService(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected void Notify(string message) 
        { 
            _notifier.Handle(new Notifications.Notification(message));
        }
        protected void Notify(ValidationResult validationResult) => validationResult.Errors.ForEach(error => Notify(error.ErrorMessage));
        protected bool IsEntityValid<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE>
            where TE : Entity
        {
            var validator = validation.Validate(entity);

            if(validator.IsValid)
                return true;

            Notify(validator);

            return false;
        }
    }
}
