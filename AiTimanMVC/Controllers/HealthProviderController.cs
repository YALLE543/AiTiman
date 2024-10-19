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
using System.Globalization;

namespace AiTimanMVC.Controllers
{
    public class HealthProviderController : BaseController
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
                HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Appointment/UpdateAppointment/Update-Appointment?id=" + id, content).Result;
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


        public async Task<IActionResult> AppointmentsList()
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


        //[HttpGet]
        //public IActionResult Appointment()
        //{
        //    var model = new AppointmentViewModel
        //    {
        //        AppointmentSetter = User.FindFirstValue(ClaimTypes.Name) // Get the logged-in user's username
        //    };
        //    return View(model);
        //}

        [HttpGet]
        public async Task<IActionResult> Appointment()
        {
            // Fetch the appointment dates from the API
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "/Appointment/GetAppointmentDates/GetAppointmentDates");
            List<DateTime> appointmentDates = new List<DateTime>();

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                appointmentDates = JsonConvert.DeserializeObject<List<DateTime>>(data);
            }

            // Fetch time slot booking information (assuming this is available via a different API or method)
            HttpResponseMessage timeSlotResponse = await _client.GetAsync(_client.BaseAddress + "/Appointment/GetTimeSlotBookings");
            Dictionary<string, AppointmentViewModel.TimeSlotViewModel> timeSlots = new Dictionary<string, AppointmentViewModel.TimeSlotViewModel>();

            // Default schedule time range
            var defaultScheduleTime = new AppointmentViewModel.TimeRangeViewModel
            {
                StartTime = TimeSpan.FromHours(9),  // 9 AM
                EndTime = TimeSpan.FromHours(17)    // 5 PM
            };

            if (timeSlotResponse.IsSuccessStatusCode)
            {
                string timeSlotData = await timeSlotResponse.Content.ReadAsStringAsync();

                // Updated deserialization to handle complex structure
                var rawTimeSlots = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(timeSlotData);

                foreach (var slot in rawTimeSlots)
                {
                    string scheduleTimeStr = slot["scheduleTime"].ToString();  // Example: "10:00 AM - 3:00 PM"
                    var timeRange = ParseTimeRange(scheduleTimeStr);

                    // Check if booking count exists, otherwise default to 0
                    int bookingCount = slot.ContainsKey("bookingCount") ? Convert.ToInt32(slot["bookingCount"]) : 0;

                    timeSlots.Add(scheduleTimeStr, new AppointmentViewModel.TimeSlotViewModel
                    {
                        TimeRange = new AppointmentViewModel.TimeRangeViewModel
                        {
                            StartTime = timeRange.Item1,
                            EndTime = timeRange.Item2
                        },
                        BookingCount = bookingCount // Map booking count from API response
                    });
                }
            }
            else
            {
                // If no time slots data is returned, generate based on the default schedule time range
                DateTime startTime = DateTime.Today.Add(defaultScheduleTime.StartTime);
                DateTime endTime = DateTime.Today.Add(defaultScheduleTime.EndTime);

                endTime = endTime.AddMinutes(-60);
                // Initialize time slots with a 30-minute interval
                while (startTime < endTime)
                {
                    timeSlots.Add(startTime.ToString("h:mm tt"), new AppointmentViewModel.TimeSlotViewModel
                    {
                        TimeRange = new AppointmentViewModel.TimeRangeViewModel
                        {
                            StartTime = startTime.TimeOfDay,
                            EndTime = startTime.AddMinutes(60).TimeOfDay // 30-minute interval
                        },
                        BookingCount = 0 // Initialize with 0 bookings
                    });
                    startTime = startTime.AddMinutes(60);
                    // 30-minute interval
                }
            }

            // Create a new instance of the view model
            var appointmentViewModel = new AppointmentViewModel
            {
                AppointmentDates = appointmentDates,
                TimeSlots = timeSlots, // This should now correctly map to TimeSlotViewModel
                ScheduleTime = defaultScheduleTime
            };

            // Pass the populated model to the view
            return View(appointmentViewModel);
        }

        private (TimeSpan, TimeSpan) ParseTimeRange(string timeRangeStr)
        {
            // Parse the string to separate start and end times
            var times = timeRangeStr.Split(" - ");
            TimeSpan startTime = DateTime.Parse(times[0]).TimeOfDay;
            TimeSpan endTime = DateTime.Parse(times[1]).TimeOfDay;

            return (startTime, endTime);
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
                // Set the appointment setter to the current user
                model.AppointmentSetter = User.FindFirstValue(ClaimTypes.Name);

                // Handle ScheduleDate to ensure it's in UTC format
                DateTime scheduleDate;

                if (DateTime.TryParseExact(model.ScheduleDate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out scheduleDate))
                {
                    // Convert the parsed date to UTC
                    DateTime utcScheduleDate = scheduleDate.ToLocalTime();

                    // If model.ScheduleDate is a DateTime, assign the formatted date
                    model.ScheduleDate = utcScheduleDate; // Assuming model.ScheduleDate is a DateTime
                }

                // Serialize the model and send it to the API
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Appointment/CreateNewAppointment/Create-New-Appointment", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Appointment Successfully Set";
                    return RedirectToAction("AppointmentsList");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "An error occurred while processing your request. Please try again.";
                // Log the exception details here for debugging
                // _logger.LogError(ex, "Error occurred while creating the appointment.");
            }

            TempData["errorMessage"] = "An error occurred while creating the appointment.";
            return View(model); // Pass the model back to the view to show validation errors
        }
    }
}