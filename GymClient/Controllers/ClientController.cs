using Microsoft.AspNetCore.Mvc;

namespace GymClient.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            Console.WriteLine(User.Identity.Name);
            
            return View();
        }
    }
}
