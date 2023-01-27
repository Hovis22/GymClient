﻿using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System.Numerics;
using System.Text.Json;
using System.Text;
using GymClient.Models;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Newtonsoft.Json;

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
           // ViewBag.IsEmail = isEmail;

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
         
           if(await SendRegisterModel(reg) == false)
            {
                return Index();
            }
           
            return Redirect("/");
        }



        public async Task<bool> SendRegisterModel(RegisterModel registerModel)
        {
            string url = "register";


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
                    registerModel = JsonConvert.DeserializeObject<RegisterModel>(jsonString);


                    Authenticate(registerModel);

                    return true;

                }
              

            }

            return false;
        }






        private async Task Authenticate(RegisterModel model)
        {
         
       
            var claims = new List<Claim>
            {

                new Claim(ClaimsIdentity.DefaultNameClaimType, model.Name),
            
                new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
             
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);


            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));


        }



    }

}
