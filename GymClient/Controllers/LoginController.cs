using GymClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GymClient.Controllers
{
    public class LoginController : Controller
    {
        IHttpClientFactory clientFactory = null;

        
        public LoginController(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public IActionResult Index(string? error = "")
        {

           ViewBag.Error = error;
           Console.WriteLine(ViewBag.Error);
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> LogIn(LoginModel reg)
        {

            if (await SendLoginModel(reg) == false)
            {
                return RedirectToAction("Index", "Login", new { error="Email or Password Incorrect" });
            }


            return Redirect("/");
        }



        public async Task<bool> SendLoginModel(LoginModel registerModel)
        {
            string url = "auth/login";


            var client = clientFactory.CreateClient(
            name: "Gym");

            var registermodelJson = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(registerModel),
                Encoding.UTF8,
                Application.Json);

            using (var httpResponseMessage =
                 await client.PostAsync(url, registermodelJson))
            {

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonString = await httpResponseMessage.Content.ReadAsStringAsync();
                    var user  = JsonConvert.DeserializeObject<Client>(jsonString);


                    await Authenticate(user);

                    return true;

                }


            }

            return false;
        }







        private async Task Authenticate(Client model)
        {


            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, model.Email),
        new Claim(ClaimTypes.Locality, model.Name),
        new Claim(ClaimTypes.NameIdentifier, model.Id.ToString())
    };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


            await HttpContext.SignInAsync(claimsPrincipal);


        }





    }
}
