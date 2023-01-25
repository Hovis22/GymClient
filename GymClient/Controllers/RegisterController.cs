using Microsoft.AspNetCore.Mvc;

namespace GymClient.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
