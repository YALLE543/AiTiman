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

    public class UsersController : BaseController
    {
        Uri baseAddress = new Uri("https://localhost:7297/api"); // Use the correct port here
        private readonly HttpClient _client;
        private readonly VerificationService _verificationService;
        private readonly IEmailSender _emailSender;
        private readonly IUsers _userService;

        public UsersController(VerificationService verificationService, IEmailSender emailSender, IUsers userService)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            _verificationService = verificationService;
            _emailSender = emailSender;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile(string id)
        {
            try
            {
                var userId = User.FindFirstValue("UserId");
                UsersViewModel users = new UsersViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Users/FetchUsers/Fetch-Users?id=" + userId).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    users = JsonConvert.DeserializeObject<UsersViewModel>(data);
                }
                return View(users);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UsersViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Users/CreateNewUsers/Create-New-Users", content).Result;

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
        public IActionResult Login()
        {
            return View();
        }





        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Call your API to validate the user credentials
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Users/ValidateLogin/ValidateLogin", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var userResponse = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserResponseModel>(userResponse); // Deserialize into the correct model

                    // Create user claims
                    var claims = new List<Claim>
            {
                new Claim("UserId", user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email)
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Sign in the user
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // Redirect based on role
                    switch (user.Role)
                    {
                        case "Admin":
                            return RedirectToAction("AdminDashboard", "Admin");
                        case "HealthProvider":
                            return RedirectToAction("HealthProviderDashboard", "HealthProvider");
                        case "Patient":
                            return RedirectToAction("PatientDashboard", "Patient");
                        default:
                            // Redirect to home if role is unknown
                            return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Invalid login attempt.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(model);
            }
        }

        //[HttpGet("profile")]
        //public IActionResult GetProfile()
        //{
        //    Return user profile data
        //    var userProfile = // fetch user profile data
        //return Ok(userProfile);
        //}

        //public IActionResult UserProfile()
        //{
        //    Fetch user profile using the correct method name
        //    var userProfile = _userService.GetUserProfileByUsername(User.Identity.Name); // Use GetUserProfileByUsername

        //    if (userProfile == null)
        //    {
        //        Handle the case where the user is not found
        //        return NotFound();
        //    }

        //    return View(userProfile);
        //}
    }
}