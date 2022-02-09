using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityNetCore.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(policy:"DepAdmin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
