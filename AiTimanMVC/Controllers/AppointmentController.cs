using AiTimanMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AiTimanMVC.Controllers
{
    public class AppointmentController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44361/api");
        private readonly HttpClient _client;

        public AppointmentController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
                    
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<AppointmentViewModel> appointmentList = new List<AppointmentViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/appointment/Get").Result;
            
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                appointmentList = JsonConvert.DeserializeObject<List<AppointmentViewModel>>(data);
            }
            return View(appointmentList);
        }
    }
}
