using IdentityNetCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityNetCore.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<IdentityController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
                if (!(await _roleManager.RoleExistsAsync(model.Role)))
                {
                    var role = new IdentityRole()
                    {
                        Name = model.Role
                    };

                    var roleResult = await _roleManager.CreateAsync(role);
                    if (!roleResult.Succeeded)
                    {
                        var errors = roleResult.Errors.Select(i => i.Description);
                        if (errors != null)
                            ModelState.AddModelError("addRole", string.Join("", errors));

                        return View(model);
                    }
                }

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
                        await _userManager.AddClaimAsync(user, new Claim("Department", model.Department));

                        await _userManager.AddToRoleAsync(user,model.Role);

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
        public  IActionResult Signin()
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

                var result = await _signInManager.PasswordSignInAsync(identityUser, model.Password, model.RememberMe,false);
                var roles = await _userManager.GetRolesAsync(identityUser);

                if (result.Succeeded)
                    return RedirectToAction("Index","Home");
                else ModelState.AddModelError("Login", "login failed");

            }
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> Signout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index","Home");
        }
    }
}
