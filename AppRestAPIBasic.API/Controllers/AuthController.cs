using AppRestAPIBasic.Api.ViewModels;
using AppRestAPIBasic.Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppRestAPIBasic.API.Controllers
{
    [Route("api/auth")]
    public class AuthController : MainController
    {
        readonly SignInManager<IdentityUser> _signInManager;
        readonly UserManager<IdentityUser> _userManager;
        public AuthController(INotifier notifier, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager) : base(notifier)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserViewModel registerUserViewModel)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new IdentityUser()
            {
                UserName = registerUserViewModel.Email,
                Email = registerUserViewModel.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUserViewModel.Password);

            if (result.Succeeded)
                await _signInManager.SignInAsync(user, false);
            else
                result.Errors.ToList().ForEach(error => NotifyError(error.Description));            

            return CustomResponse(registerUserViewModel);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserViewModel loginUserViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUserViewModel.Password, loginUserViewModel.Password, false, true);

            if (result.IsLockedOut)
                NotifyError("User temporary blocked by multiple login atempts");

            if (!result.Succeeded)
                NotifyError("User or Password incorrect.");

            return CustomResponse(loginUserViewModel);
        }
    }
}
