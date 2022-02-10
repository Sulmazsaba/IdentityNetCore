using IdentityNetCore.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<List<ProductViewModel>> Get()
        {
            var products = new List<ProductViewModel>()
            {
                new ProductViewModel()
                {
                     Id = 1,
                     Name = "Mobile"
                },
                new ProductViewModel()
                {
                    Name ="Computer"
                }
            };
            return Ok(products);
        }
    }
}
