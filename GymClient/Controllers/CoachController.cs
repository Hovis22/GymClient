using GymClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GymClient.Controllers
{
    public class CoachController : Controller
    {IHttpClientFactory clientFactory;
		IWebHostEnvironment _appEnvironment;

		public CoachController(IHttpClientFactory clientFactory, IWebHostEnvironment appEnvironment)
		{
			this.clientFactory = clientFactory;
			_appEnvironment = appEnvironment;
		}


		public async Task<IActionResult> Index()
		{
			
			ViewBag.Coaches = await OnGetCoachesAsync();

			return View();
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
