using GymClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GymClient.Controllers
{
    public class ClassesController : Controller
    {

		IHttpClientFactory clientFactory;
		IWebHostEnvironment _appEnvironment;

		public ClassesController(IHttpClientFactory clientFactory, IWebHostEnvironment appEnvironment)
		{
			this.clientFactory = clientFactory;
			_appEnvironment = appEnvironment;
		}



		public async Task<IActionResult> Index()
        {
			ViewBag.Schedule = await OnGetScheduleAsync();

            return View();
        }




		public async Task<List<Schedule>?> OnGetScheduleAsync()
		{

			string uri = "admin/getschedule";

			var client = clientFactory.CreateClient(
			name: "Gym");
			var request = new HttpRequestMessage(
			method: HttpMethod.Get, requestUri: uri);
			HttpResponseMessage response = await client.SendAsync(request);
			if (response.IsSuccessStatusCode)
			{
				string jsonString = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<IEnumerable<Schedule>>(jsonString).ToList<Schedule>();
			}
			return null;
		}









	}
}
