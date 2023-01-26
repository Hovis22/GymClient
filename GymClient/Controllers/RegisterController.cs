using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System.Numerics;
using System.Text.Json;
using System.Text;
using GymClient.Models;
using System;

namespace GymClient.Controllers
{

    
    public class RegisterController : Controller
    {

        IHttpClientFactory clientFactory;

        public RegisterController(IHttpClientFactory httpClientFactory)
        {
            clientFactory = httpClientFactory;

        }



        public IActionResult Index()
        {
            return View();
        }




        //    [HttpPost]
        //    public async Task<IActionResult> OnPostAsync([Bind("Id,Name,LastName,BirthDay,Phone,Email,Sex,Password")] RegisterModel reg)
        //{
        //    Console.WriteLine(123);
        //    return Redirect("/");
        //}




        [HttpPost]
        public async Task<IActionResult> GoNew(RegisterModel reg) {
            Console.WriteLine(reg.BirthDay);

            SendRegisterModel(reg);

            return Redirect("/Register");
        }



        public async void SendRegisterModel(RegisterModel registerModel)
        {
            string url = "register";


            var client = clientFactory.CreateClient(
            name: "Gym");

            var registermodelJson = new StringContent(
                JsonSerializer.Serialize(registerModel),
                Encoding.UTF8,
                Application.Json);

            using (var httpResponseMessage =
                await client.PostAsync(url, registermodelJson))  
            {

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                 

                    ViewData["Rezult"] = "Ok";
              //      await Authenticate(st);

                }
                else ViewData["Rezult"] = $"Incorrect Data";
              //  return Page();

            }






        }






    }

}
