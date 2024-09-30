using AiTimanMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AiTimanMVC.Controllers
{
    public class AppointmentController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7297/api");
        private readonly HttpClient _client;

        public AppointmentController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;

        }

        [HttpGet]
        public IActionResult Appointment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Appointment(AppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["errorMessage"] = "Model is invalid. Please ensure all fields are correctly filled.";
                return View(model); // Return the same model to populate the view with existing data
            }

            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Appointment/CreateNewAppointment/Create-New-Appointment", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Appointment Successfully Set";
                    return RedirectToAction("AdminDashboard", "Admin");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "An error occurred while processing your request. Please try again.";
                // Log exception here
            }

            TempData["errorMessage"] = "An error occurred while creating the appointment.";
            return View(model); // Pass the model back to the view to show validation errors
        }
    }
}
