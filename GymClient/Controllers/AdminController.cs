using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymClient.Controllers
{
    public class AdminController : Controller
    {

        [Authorize(Policy = "OnlyForAdmins")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "OnlyForAdmins")]
        public IActionResult AddCoach()
        {
            return View();
        }




    }
}
