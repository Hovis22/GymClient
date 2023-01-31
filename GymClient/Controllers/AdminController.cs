using GymClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Numerics;
using System.Security.Claims;

namespace GymClient.Controllers
{
    public class AdminController : Controller
    {

		IHttpClientFactory clientFactory;
		IWebHostEnvironment _appEnvironment;

		public AdminController(IHttpClientFactory clientFactory, IWebHostEnvironment appEnvironment)
		{
			this.clientFactory = clientFactory;
			_appEnvironment = appEnvironment;
		}





		[Authorize(Policy = "OnlyForAdmins")]
        public IActionResult Index()
        {
            return View();
        }

		public async Task<IActionResult> Coach()
		{
			ViewBag.Sch =await SendCoachId(Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value));

			return View();
		}


		[Authorize(Policy = "OnlyForAdmins")]
        public IActionResult AddCoach()
        {
            return View();
        }


		public async Task<IActionResult> AddScheldue()
		{
			ViewBag.Coaches = await OnGetCoachesAsync();
			ViewBag.Users = await OnGetUsersAsync();
			ViewBag.RoleId = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
			ViewBag.UserId = Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);
			return View();
		}




		[HttpPost]
		public async Task<IActionResult> AddNewPersonal(Personal reg, IFormFile uploadImage)
		{

			byte[] imageData = null;

			string path = "/PersonalImg/" + uploadImage.FileName;

			using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
			{
				await uploadImage.CopyToAsync(fileStream);
			}

			reg.IMGPath = path;

			if (await SendPersonalModel(reg) == false)
			{
				return RedirectToAction("AddCoach");
			};
			


			return Redirect("/");
		}




		[HttpPost]
		public async Task<IActionResult> AddNewScheldue(Schedule reg)
		{
			Console.WriteLine(reg.TypeId);


			if (await SendScheduleModel(reg) == false)
			{
				return RedirectToAction("AddScheldue");
			};



			return Redirect("/");
		}

		public async Task<bool> SendScheduleModel(Schedule schedule)
		{
			string url = "admin/addschedule";

			Console.WriteLine(1234);
			var client = clientFactory.CreateClient(
			name: "Gym");

			var registermodelJson = new StringContent(
				System.Text.Json.JsonSerializer.Serialize(schedule),
				Encoding.UTF8,
				Application.Json);

			using (var httpResponseMessage =
				 await client.PostAsync(url, registermodelJson))
			{

				if (httpResponseMessage.IsSuccessStatusCode)
				{


					return true;

				}


			}

			return false;
		}








		public async Task<bool> SendPersonalModel(Personal loginModel)
		{
			string url = "admin/addpersonal";

			
			var client = clientFactory.CreateClient(
			name: "Gym");

			var registermodelJson = new StringContent(
				System.Text.Json.JsonSerializer.Serialize(loginModel),
				Encoding.UTF8,
				Application.Json);

			using (var httpResponseMessage =
				 await client.PostAsync(url, registermodelJson))
			{

				if (httpResponseMessage.IsSuccessStatusCode)
				{
					

					return true;

				}


			}

			return false;
		}


		public async Task<List<Personal>?> OnGetCoachesAsync()
		{

			string uri = "admin/getcoach";

			var client = clientFactory.CreateClient(
			name: "Gym");
			var request = new HttpRequestMessage(
			method: HttpMethod.Get, requestUri: uri);
			HttpResponseMessage response = await client.SendAsync(request);
			if (response.IsSuccessStatusCode)
			{
				string jsonString = await response.Content.ReadAsStringAsync();
				 return JsonConvert.DeserializeObject<IEnumerable<Personal>>(jsonString).ToList<Personal>();
			}
			return null;
		}


		public async Task<List<Client>?> OnGetUsersAsync()
		{

			string uri = "admin/getusers";

			var client = clientFactory.CreateClient(
			name: "Gym");
			var request = new HttpRequestMessage(
			method: HttpMethod.Get, requestUri: uri);
			HttpResponseMessage response = await client.SendAsync(request);
			if (response.IsSuccessStatusCode)
			{
				string jsonString = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<IEnumerable<Client>>(jsonString).ToList<Client>();
			}
			return null;
		}



		public async Task<List<Schedule>?> SendCoachId(int id)
		{
			UserId userId = new UserId();
			Console.Write(id);
			string url = "admin/getcoachschedule";

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
}
