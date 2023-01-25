using Microsoft.AspNetCore.Mvc;

namespace GymClient.Controllers
{
    public class CoachController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
