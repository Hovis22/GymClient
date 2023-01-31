using GymClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Drawing;

namespace GymClient.Controllers
{
	public class UserController : Controller
	{

		IHttpClientFactory clientFactory = null;


		public UserController(IHttpClientFactory clientFactory)
		{
			this.clientFactory = clientFactory;
		}



		[Authorize]
		public async Task<IActionResult> Index()
		{
			string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
			int id = Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);
			ViewBag.Schedule = await SendUserId(id, "user/getusertrain");
			ViewBag.ScheduleSolo = await SendUserId(id, "user/getusersolo");
			ViewBag.Name = User.Identity.Name;
			switch (role)
			{
				case "1": return RedirectToAction("Coach", "Admin");
				case "2": return RedirectToAction("Index", "Admin");


			}
			return View();
		}



		public IActionResult LogOut()
		{


			Response.Cookies.Delete(".AspNetCore.Cookies");

		  return	 Redirect("/");
		}


		public async Task<List<Schedule>?> SendUserId(int id,string url)
		{
			//string url = "user/getusertrain";

			UserId userId = new UserId();

			userId.Id = id;


			var client = clientFactory.CreateClient(
			name: "Gym");

			var registermodelJson = new StringContent(
				System.Text.Json.JsonSerializer.Serialize(userId),
				Encoding.UTF8,
				Application.Json);

			using (var httpResponseMessage =
				 await client.PostAsync(url, registermodelJson))
			{

				if (httpResponseMessage.IsSuccessStatusCode)
				{

					string jsonString = await httpResponseMessage.Content.ReadAsStringAsync();
					return JsonConvert.DeserializeObject<IEnumerable<Schedule>>(jsonString).ToList<Schedule>();

				}


			}

			return null;
		}







	}


	class UserId{

		public int Id { get; set; }
	}

}
