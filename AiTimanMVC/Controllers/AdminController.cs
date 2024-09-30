using AiTimanMVC.Models;
using AiTimanMVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using AiTiman_API.Services.DTO;
using AiTiman_API.Services.Interfaces;
using AiTiman_API.Services.Respositories;
using AiTiman_API.Models;

namespace AiTimanMVC.Controllers
{
    public class AdminController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7297/api"); // Use the correct port here
        private readonly HttpClient _client;

        // Constructor to inject IHttpClientFactory and initialize HttpClient
        public AdminController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;// Ensure the correct port
        }

        // Admin Dashboard Action
        public IActionResult AdminDashboard()
        {
            //string userName = User.Identity.Name;  // Get the logged-in user's username

            //Call the API to get the user profile
            //var response = await _client.GetAsync($"users/GetUserProfile/{userName}");

            //if (!response.IsSuccessStatusCode)
            //{
            //    return View("Error"); // Handle error accordingly
            //}

            //Read the user profile data from the response
            //var userResponse = await response.Content.ReadAsStringAsync();
            //var user = JsonConvert.DeserializeObject<UserProfileViewModel>(userResponse);

            //Pass the user profile to the view
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
