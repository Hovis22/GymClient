using GymClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

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



		public async Task<IActionResult> Index(string? error = "")
        {
			ViewBag.Schedule = await OnGetScheduleAsync();
			ViewBag.Error = error;
            return View();
        }



		public async Task<IActionResult> UserToTrain(int id)
		{
	      if(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier) == (null))
	 		{
				return RedirectToAction("Index", "Login");
			}



			int? userId = Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);

		
	




			if (await SendUserToTrainModel(id, userId) == false)
			{
				return RedirectToAction("Index", "Classes", new { error = "Allready In" });
			}


			return RedirectToAction("Index", "User");
		}





		public async Task<bool> SendUserToTrainModel(int id,int? userid)
		{
			string url = "classes/usert";

			PeopleOnWorkouts peopleOn = new PeopleOnWorkouts(userid,id);
			Console.WriteLine(peopleOn.ClientId);
			Console.WriteLine(peopleOn.ScheduledId);

			var client = clientFactory.CreateClient(
			name: "Gym");

			var registermodelJson = new StringContent(
				System.Text.Json.JsonSerializer.Serialize(peopleOn),
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
