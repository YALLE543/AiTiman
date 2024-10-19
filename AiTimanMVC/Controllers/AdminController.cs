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
    public class AdminController : BaseController
    {
        Uri baseAddress = new Uri("https://localhost:7297/api"); // Use the correct port here
        private readonly HttpClient _client;

        // Constructor to inject IHttpClientFactory and initialize HttpClient
        public AdminController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;// Ensure the correct port
        }

        public IActionResult AdminDashboard()
        {
            return View();
            
        }

         public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminAccountList()
        {
            List<UsersViewModel> userslist = new List<UsersViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Users/AllUsers/All-Users").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                userslist = JsonConvert.DeserializeObject<List<UsersViewModel>>(data);
            }

            return View(userslist);
        }

        
    }
}
