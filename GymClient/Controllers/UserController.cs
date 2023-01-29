using GymClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymClient.Controllers
{
    public class UserController : Controller
    {


        [Authorize(Policy = "Clients")]
        public IActionResult Index()
        {
            Console.WriteLine(User.Identity.Name);
            
            return View();
        }
    }
}
