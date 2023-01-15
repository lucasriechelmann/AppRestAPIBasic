using System.Runtime.InteropServices.ComTypes;
using AppRestAPIBasic.Business.Interfaces;
using AppRestAPIBasic.Business.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppRestAPIBasic.API.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotifier _notifier;
        protected readonly IUser AppUser;
        protected Guid UserId { get; set; }
        protected bool UserAuthenticated { get; set; }
        protected MainController(INotifier notifier, IUser appUser)
        {
            _notifier = notifier;
            AppUser = appUser;

            if (appUser.IsAuthenticated())
            {
                UserId = appUser.GetUserId();
                UserAuthenticated = true;
            }
        }
        protected bool IsRequestValid() => !_notifier.IsThereNotification();
        protected IActionResult CustomResponse(object result = null)
        {
            if (IsRequestValid())
                return Ok(new { success = true, data = result });
                        
            return BadRequest(new { uccess = true, errors = _notifier.GetNotifications().Select(x => x.Message) });
        }
        protected IActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if(!modelState.IsValid)
                return CustomResponse();

            return Ok();
        }

        protected void NotifyErrorModelInvalid(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);

            foreach (var error in errors)
            {
                var errorMessage = error.Exception is null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(errorMessage);
            }
        }

        protected void NotifyError(string errorMessage) => _notifier.Handle(new Notification(errorMessage));
    }
}