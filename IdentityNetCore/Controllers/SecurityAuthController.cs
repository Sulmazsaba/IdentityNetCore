using IdentityNetCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityAuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly UserManager<IdentityUser> _userManager;

        public SecurityAuthController(IConfiguration configuration, SignInManager<IdentityUser> signinManager, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _signinManager = signinManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [Route("Token")]
        public async Task<IActionResult> GetToken(SigninViewModel model)
        {
            var issuer = _configuration["Tokens:Issuer"];
            var audience = _configuration["Tokens:Audience"];
            var key = _configuration["Tokens:Key"];

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.UserName);
                if (user == null) return NotFound();

                var result = await _signinManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Email,model.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti , user.Id)
                    };

                    var keyBytes = Encoding.UTF8.GetBytes(key);
                    var symmetriyKey = new SymmetricSecurityKey(keyBytes);
                    var credentials = new SigningCredentials(symmetriyKey, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(issuer, audience, claims, DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(30), credentials);

                    var tokenRes = new { token = new JwtSecurityTokenHandler().WriteToken(token) };
                    return Ok(tokenRes);
                }
            }
            return BadRequest();
        }
    }

}


