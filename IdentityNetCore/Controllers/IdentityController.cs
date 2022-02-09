using IdentityNetCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityNetCore.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<IdentityController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IdentityController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Signup()
        {
            var model = new SignUpViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {

                if ((await _userManager.FindByEmailAsync(model.Email)) == null)
                {
                    var user = new IdentityUser
                    {
                        Email = model.Email,
                        UserName = model.Email
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Signin");
                    }

                    ModelState.AddModelError("signup", String.Join("/n", result.Errors.Select(err => err.Description)));
                    return View(model);
                }

            }
            return View(model);
        }

        public async Task<IActionResult> ConfirmByEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return View("Error");

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                return new NotFoundResult();

            return RedirectToAction("Signin");
        }
        public IActionResult Signin()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Signin(SigninViewModel model)
        {
            if (ModelState.IsValid)
            {

                var identityUser = await _userManager.FindByEmailAsync(model.UserName);
                if (identityUser == null) return new NotFoundResult();

                var result = await _signInManager.PasswordSignInAsync(identityUser, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                    return RedirectToAction("/Home/Index");
                else ModelState.AddModelError("Login", "login failed");

            }
            return View();
        }

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}
