using AiTiman_API.Models;
using AiTimanMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AiTimanMVC.Controllers
{
    public class PatientController : BaseController
    {
        private readonly HttpClient _client;

        public PatientController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7297/api"); // Adjust the port accordingly
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PatientDashboard()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddBooking(string id)
        {
            var booked = new BookedViewModel();

            try
            {
                // Fetch appointment details
                booked.AppointmentModel = await FetchAppointmentDetails(id);

                // Fetch user details
                var userId = User.FindFirstValue("UserId");
                booked.UsersModel = await FetchUserDetails(userId);

                // Fetch available appointment dates
                //booked.AppointmentModel.AppointmentDates = await FetchAvailableAppointmentDates();

                //// Fetch available time slots for booking
                //booked.AppointmentModel.TimeSlots = await FetchAvailableTimeSlots();

                // Set the default schedule time
                //booked.AppointmentModel.ScheduleTime = new AppointmentViewModel.TimeRangeViewModel
                //{
                //    StartTime = TimeSpan.FromHours(9),  // 9 AM
                //    EndTime = TimeSpan.FromHours(17)    // 5 PM
                //};

                return View(booked);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        private async Task<AppointmentViewModel> FetchAppointmentDetails(string id)
        {
            var response = await _client.GetAsync($"{_client.BaseAddress}/Appointment/FetchAppointment/Fetch-Appointment?id={id}");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AppointmentViewModel>(data);
            }

            return null; // Or handle appropriately
        }

        private async Task<UsersViewModel> FetchUserDetails(string userId)
        {
            var response = await _client.GetAsync($"{_client.BaseAddress}/Users/FetchUsers/Fetch-Users?id={userId}");

            if (response.IsSuccessStatusCode)
            {
                string userData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UsersViewModel>(userData);
            }

            return null; // Or handle appropriately
        }

        private async Task<List<DateTime>> FetchAvailableAppointmentDates()
        {
            var response = await _client.GetAsync($"{_client.BaseAddress}/Appointment/GetAppointmentDates/GetAppointmentDates");

            if (response.IsSuccessStatusCode)
            {
                string dateData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DateTime>>(dateData);
            }

            return new List<DateTime>(); // Return an empty list if no dates available
        }

        private async Task<Dictionary<string, AppointmentViewModel.TimeSlotViewModel>> FetchAvailableTimeSlots()
        {
            var response = await _client.GetAsync($"{_client.BaseAddress}/Appointment/GetTimeSlotBookings");
            var timeSlots = new Dictionary<string, AppointmentViewModel.TimeSlotViewModel>();

            if (response.IsSuccessStatusCode)
            {
                string timeSlotData = await response.Content.ReadAsStringAsync();
                var rawTimeSlots = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(timeSlotData);

                foreach (var slot in rawTimeSlots)
                {
                    string scheduleTimeStr = slot["scheduleTime"].ToString();
                    var timeRange = ParseTimeRange(scheduleTimeStr);
                    int bookingCount = slot.ContainsKey("bookingCount") ? Convert.ToInt32(slot["bookingCount"]) : 0;

                    timeSlots.Add(scheduleTimeStr, new AppointmentViewModel.TimeSlotViewModel
                    {
                        TimeRange = new AppointmentViewModel.TimeRangeViewModel
                        {
                            StartTime = timeRange.Item1,
                            EndTime = timeRange.Item2
                        },
                        BookingCount = bookingCount
                    });
                }
            }
            else
            {
                // If no time slot data is available, create default slots
                CreateDefaultTimeSlots(timeSlots);
            }

            return timeSlots;
        }

        private void CreateDefaultTimeSlots(Dictionary<string, AppointmentViewModel.TimeSlotViewModel> timeSlots)
        {
            var defaultScheduleTime = new AppointmentViewModel.TimeRangeViewModel
            {
                StartTime = TimeSpan.FromHours(9),  // 9 AM
                EndTime = TimeSpan.FromHours(17)    // 5 PM
            };

            DateTime startTime = DateTime.Today.Add(defaultScheduleTime.StartTime);
            DateTime endTime = DateTime.Today.Add(defaultScheduleTime.EndTime).AddMinutes(-60);

            while (startTime < endTime)
            {
                timeSlots.Add(startTime.ToString("h:mm tt"), new AppointmentViewModel.TimeSlotViewModel
                {
                    TimeRange = new AppointmentViewModel.TimeRangeViewModel
                    {
                        StartTime = startTime.TimeOfDay,
                        EndTime = startTime.AddMinutes(60).TimeOfDay
                    },
                    BookingCount = 0
                });
                startTime = startTime.AddMinutes(60);
            }
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
        public async Task<IActionResult> AddBooking(BookedViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Log model errors for debugging
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    Console.WriteLine(error); // or use a logger
                }

                TempData["errorMessage"] = "Model is invalid. Please ensure all fields are correctly filled.";
                return View(model);  // Re-display the form with validation errors
            }
            try
            {
                // Serialize the model to JSON for sending via HTTP POST
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                // Make the POST request to the booking API
                HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "/Booked/CreateNewBooked/Create-New-Booked", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Successfully Booked Appointment";
                    return RedirectToAction("BookedAppointments");
                }
            }
            catch (Exception ex)
            {
                // Log the exception details here for debugging
                TempData["errorMessage"] = "An error occurred while processing your request. Please try again.";
            }

            // If the booking was not successful, return to the view with an error message
            TempData["errorMessage"] = "An error occurred while creating the appointment.";
            return View(model);  // Re-display the form
        }


        public async Task<IActionResult> Booking()
        {
            var appointmentList = new List<AppointmentViewModel>();
            var response = await _client.GetAsync(_client.BaseAddress + "/Appointment/AllAppointments/All-Appointments");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                appointmentList = JsonConvert.DeserializeObject<List<AppointmentViewModel>>(data);
            }

            return View(appointmentList);
        }


        [HttpGet]
        public IActionResult BookedAppointments()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BookedAppointments(BookingViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Booked/CreateNewBooked/Create-New-Booked", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "User Account Successfully Registered";
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex;
                return View();
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetAppointmentDetails(string id)
        {
            var appointment = new AppointmentViewModel();
            try
            {
                var response = await _client.GetAsync($"/Appointment/FetchAppointment/Fetch-Appointment?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    appointment = JsonConvert.DeserializeObject<AppointmentViewModel>(data);
                    return Json(appointment);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error fetching appointment details: {ex.Message}" });
            }
            return BadRequest(new { message = "Appointment not found." });
        }


    }
}
