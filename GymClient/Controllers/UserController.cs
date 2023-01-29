using GymClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymClient.Controllers
{
    public class UserController : Controller
    {


        [Authorize]
        public IActionResult Index()
        {
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

            switch (role)
            {
                case "1": return RedirectToAction("Index","Admin");
               case "2": return RedirectToAction("Index", "Admin");


            }
            return View();
        }




      







    }
}
