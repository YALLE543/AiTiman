using AiTimanMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;

namespace AiTimanMVC.Controllers
{
    public class BaseController : Controller
    {
        protected readonly HttpClient _client;

        public BaseController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:7297/api"); // Adjust the base address as necessary
        }

        protected UsersViewModel GetCurrentUserInfo()
        {
            var userId = User.FindFirstValue("UserId"); // Fetch UserId from claims

            if (string.IsNullOrEmpty(userId))
                return null;

            // Call the API to get the user information based on userId
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Users/FetchUsers/Fetch-Users?id=" + userId).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<UsersViewModel>(data);
            }

            return null; // Handle if user info is not found
        }

        protected void SetCurrentUserInViewBag()
        {
            var currentUser = GetCurrentUserInfo();
            ViewBag.CurrentUser = currentUser; // Pass the user info to the view using ViewBag
        }

        // Call SetCurrentUserInViewBag before each action executes
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SetCurrentUserInViewBag(); // Ensures that current user info is set before any action
            base.OnActionExecuting(context); // Call the base method to ensure normal execution
        }
    }
}
