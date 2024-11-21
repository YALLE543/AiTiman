using AiTimanMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace AiTimanMVC.Controllers
{
    public class ReferralController : BaseController
    {
        Uri baseAddress = new Uri("https://localhost:7297/api"); // Use the correct port here
        private readonly HttpClient _client;

        public ReferralController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ReferralsList() // Make this method async
        {
            List<ReferralViewModel> referrallist = new List<ReferralViewModel>();

            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Referrals/AllReferrals/All-Referrals").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                referrallist = JsonConvert.DeserializeObject<List<ReferralViewModel>>(data);
            }
            return View(referrallist);
        }


        [HttpGet]
        public IActionResult Referral()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Referral(ReferralViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["errorMessage"] = "Model is invalid. Please ensure all fields are correctly filled.";
                return View(model); // Return the same model to populate the view with existing data
            }

            try
            {

                model.DoctorInCharge = User.FindFirstValue(ClaimTypes.Name);
                //model.DateCreated = User.FindFirstValue(ClaimTypes.Name)
      
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Referrals/CreateNewReferral/Create-New-Referral", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = " Referral Successfully Set";
                    return RedirectToAction("ReferralsList", "Referral");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "An error occurred while processing your request. Please try again.";
                // Log exception here
            }

            TempData["errorMessage"] = "An error occurred while creating the  Referral.";
            return View(model); // Pass the model back to the view to show validation errors
        }


        [HttpGet]
        public IActionResult EditReferral(string id)
        {
            try
            {
                ReferralViewModel referral = new ReferralViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Referrals/FetchReferral/Fetch-Referral?id=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    referral = JsonConvert.DeserializeObject<ReferralViewModel>(data);
                }
                return View(referral);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }

        [HttpPost]
        public IActionResult EditReferral(ReferralViewModel model, string id)
        {

            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Referrals/UpdateReferral/Update-Referral?id=" + id, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = " Referral successfully updated";
                    return RedirectToAction("ReferralsList");
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
        public IActionResult DeleteReferral(string id)
        {
            try
            {
                ReferralViewModel referral = new ReferralViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Referrals/FetchReferral/Fetch-Referral?id=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    referral = JsonConvert.DeserializeObject<ReferralViewModel>(data);
                }
                return View(referral);
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
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/Referrals/DeleteReferral/Delete-Referral?id=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Referral successfully deleted";
                    return RedirectToAction("ReferralsList");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();

        }


    }
}
