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
            // Initialize the view model
            BookedViewModel booked = new BookedViewModel
            {
                AppointmentModel = new AppointmentViewModel
                {
                    TimeSlots = new Dictionary<string, AppointmentViewModel.TimeSlotViewModel>(),
                    AppointmentDates = new List<DateTime>(),
                    ScheduleTime = new AppointmentViewModel.TimeRangeViewModel()
                },
                UsersModel = new UsersViewModel()
            };

            try
            {
                // Fetch the appointment details by ID
                HttpResponseMessage response = await _client.GetAsync($"{_client.BaseAddress}/Appointment/FetchAppointment/Fetch-Appointment?id={id}");

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    booked.AppointmentModel = JsonConvert.DeserializeObject<AppointmentViewModel>(data);
                }
                else
                {
                    TempData["errorMessage"] = "Failed to fetch appointment details.";
                    return View(booked); // Return the view with an empty model
                }

                var userId = User.FindFirstValue("UserId");

                // Fetch user data
                HttpResponseMessage userResponse = await _client.GetAsync($"{_client.BaseAddress}/Users/FetchUsers/Fetch-Users?id={userId}");

                if (userResponse.IsSuccessStatusCode)
                {
                    string userData = await userResponse.Content.ReadAsStringAsync();
                    booked.UsersModel = JsonConvert.DeserializeObject<UsersViewModel>(userData);
                }

                // Fetch available appointment dates
                HttpResponseMessage dateResponse = await _client.GetAsync($"{_client.BaseAddress}/Appointment/GetAppointmentDates/GetAppointmentDates");
                List<DateTime> appointmentDates = new List<DateTime>();

                if (dateResponse.IsSuccessStatusCode)
                {
                    string dateData = await dateResponse.Content.ReadAsStringAsync();
                    appointmentDates = JsonConvert.DeserializeObject<List<DateTime>>(dateData);
                }

                // Fetch time slot booking information
                DateTime selectedDate = DateTime.Today; // Default to today or pass the correct selected date
                HttpResponseMessage timeSlotResponse = await _client.GetAsync($"{_client.BaseAddress}/Booked/GetTimeSlotBookings/GetTimeSlotBookings?appointmentDate={selectedDate:yyyy-MM-dd}");

                Dictionary<string, AppointmentViewModel.TimeSlotViewModel> timeSlots = new Dictionary<string, AppointmentViewModel.TimeSlotViewModel>();

                // Default schedule time range
                var defaultScheduleTime = new AppointmentViewModel.TimeRangeViewModel
                {
                    StartTime = TimeSpan.FromHours(9), // 9 AM
                    EndTime = TimeSpan.FromHours(17)   // 5 PM
                };

                if (timeSlotResponse.IsSuccessStatusCode)
                {
                    string timeSlotData = await timeSlotResponse.Content.ReadAsStringAsync();
                    Console.WriteLine(timeSlotData); // Log the raw JSON response

                    // Deserialize the response directly to a list of TimeSlotDto
                    var rawTimeSlots = JsonConvert.DeserializeObject<List<TimeSlotDto>>(timeSlotData);

                    foreach (var slot in rawTimeSlots)
                    {
                        Console.WriteLine($"Schedule Time: {slot.ScheduleTime}, Booking Count: {slot.BookingCount}");

                        string scheduleTimeStr = slot.ScheduleTime;  // e.g., "9:00 AM - 10:00 AM"
                        var timeRange = ParseTimeRange(scheduleTimeStr);

                        // Use the booking count from the deserialized object
                        int bookingCount = slot.BookingCount;

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
                    // If no time slots data is returned, generate based on the default schedule time range
                    DateTime startTime = DateTime.Today.Add(defaultScheduleTime.StartTime);
                    DateTime endTime = DateTime.Today.Add(defaultScheduleTime.EndTime).AddMinutes(-60); // Adjust end time so last slot ends at 5 PM

                    // Initialize time slots with a 1-hour interval
                    while (startTime < endTime)
                    {
                        string formattedTimeSlot = $"{startTime.ToString("h:mm tt")} - {startTime.AddHours(1).ToString("h:mm tt")}";
                        timeSlots.Add(formattedTimeSlot, new AppointmentViewModel.TimeSlotViewModel
                        {
                            TimeRange = new AppointmentViewModel.TimeRangeViewModel
                            {
                                StartTime = startTime.TimeOfDay,
                                EndTime = startTime.AddHours(1).TimeOfDay // 1-hour interval
                            },
                            BookingCount = 0
                        });
                        startTime = startTime.AddHours(1);  // Move to the next hour
                    }
                }

                // Populate the model with fetched data
                booked.AppointmentModel.AppointmentDates = appointmentDates;
                booked.AppointmentModel.TimeSlots = timeSlots;
                booked.AppointmentModel.ScheduleTime = defaultScheduleTime;

                // Pass the populated model to the view
                return View(booked);
            }
            catch (Exception ex)
            {
                // Handle exception and pass an error message to the view
                TempData["errorMessage"] = ex.Message;
                return View(booked); // Return the view with the populated model for better context
            }
        }

        private (TimeSpan, TimeSpan) ParseTimeRange(string timeRangeStr)
        {
            var timeRangeParts = timeRangeStr.Split('-');
            if (timeRangeParts.Length == 2)
            {
                // Parse start and end times correctly
                TimeSpan startTime = DateTime.Parse(timeRangeParts[0].Trim()).TimeOfDay;
                TimeSpan endTime = DateTime.Parse(timeRangeParts[1].Trim()).TimeOfDay;
                return (startTime, endTime);
            }
            throw new FormatException("Invalid time range format.");
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking(BookedViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    Console.WriteLine(error); // or use a logger
                }

                TempData["errorMessage"] = "Model is invalid. Please ensure all fields are correctly filled.";
                return View(model); // Re-display the form with validation errors
            }

            try
            {
                // Serialize the model to JSON for sending via HTTP POST
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                // Make the POST request to the booking API
                HttpResponseMessage response = await _client.PostAsync($"{_client.BaseAddress}/Booked/CreateNewBooked/Create-New-Booked", content);
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
            return View(model); // Re-display the form
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
