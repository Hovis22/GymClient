using GymClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Numerics;

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

        [Authorize(Policy = "OnlyForAdmins")]
        public IActionResult AddCoach()
        {
            return View();
        }


		[Authorize(Policy = "OnlyForAdmins")]
		public async Task<IActionResult> AddScheldue()
		{
			ViewBag.Coaches = await OnGetCoachesAsync();
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





	}
}
