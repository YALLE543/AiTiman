using AiTimanMVC.Models;
using AiTimanMVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using AiTiman_API.Services.DTO;
using AiTiman_API.Models;

namespace AiTimanMVC.Controllers
{
    public class HealthProviderController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7297/api"); // Use the correct port here
        private readonly HttpClient _client;

        public HealthProviderController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditAppointment(string id)
        {
            try
            {
                AppointmentViewModel appointment = new AppointmentViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Appointment/FetchAppointment/Fetch-Appointment?id=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    appointment = JsonConvert.DeserializeObject<AppointmentViewModel>(data);
                }
                return View(appointment);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            
        }

        [HttpPost]
        public IActionResult EditAppointment(AppointmentViewModel model, string id)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Appointment/UpdateAppointment/Update-Appointment?id=" + id , content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Appointment successfully updated";
                    return RedirectToAction("AppointmentsList");
                }
            }
           catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();
        }


        public async Task<IActionResult> AppointmentsList() // Make this method async
        {
            List<AppointmentViewModel> appointmentlist = new List<AppointmentViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Appointment/AllAppointments/All-Appointments").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                appointmentlist = JsonConvert.DeserializeObject<List<AppointmentViewModel>>(data);
            }
            return View(appointmentlist);
        }

        [HttpGet]
        public IActionResult DeleteAppointment(string id)
        {
            try
            {
                AppointmentViewModel appointment = new AppointmentViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Appointment/FetchAppointment/Fetch-Appointment?id=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    appointment = JsonConvert.DeserializeObject<AppointmentViewModel>(data);
                }
                return View(appointment);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            try
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/Appointment/DeleteAppointment/Delete-Appointment?id=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Appointment successfully deleted";
                    return RedirectToAction("AppointmentsList");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();

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
