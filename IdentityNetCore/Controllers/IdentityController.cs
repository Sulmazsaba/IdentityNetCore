using IdentityNetCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityNetCore.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public IdentityController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
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
        public async Task<IActionResult> Signin()
        {
            return View();

        }

        //[HttpPost]
        //public async Task<IActionResult> Signin()
        //{
        //    return View();
        //}

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}
