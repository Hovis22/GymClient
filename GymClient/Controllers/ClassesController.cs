using Microsoft.AspNetCore.Mvc;

namespace GymClient.Controllers
{
    public class ClassesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
