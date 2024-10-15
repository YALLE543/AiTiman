using AiTimanMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;


namespace AiTimanMVC.Controllers
{
    public class PrescriptionController : BaseController
    {
        Uri baseAddress = new Uri("https://localhost:7297/api"); // Use the correct port here
        private readonly HttpClient _client;

        public PrescriptionController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PrescriptionsList() // Make this method async
        {
            List<PrescriptionViewModel> prescriptionlist = new List<PrescriptionViewModel>();

            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Prescription/AllPrescriptions/All-Prescriptions").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                prescriptionlist = JsonConvert.DeserializeObject<List<PrescriptionViewModel>>(data);
            }
            return View(prescriptionlist);
        }


        [HttpGet]
        public IActionResult Prescription()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Prescription(PrescriptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["errorMessage"] = "Model is invalid. Please ensure all fields are correctly filled.";
                return View(model); // Return the same model to populate the view with existing data
            }

            try
            {

                model.ProviderName = User.FindFirstValue(ClaimTypes.Name);

                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Prescription/CreateNewPrescription/Create-New-Prescription", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = " Prescription Successfully Set";
                    return RedirectToAction("PrescriptionsList", "Prescription");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "An error occurred while processing your request. Please try again.";
                // Log exception here
            }

            TempData["errorMessage"] = "An error occurred while creating the  Prescription.";
            return View(model); // Pass the model back to the view to show validation errors
        }

        [HttpGet]
        public IActionResult EditPrescription(string id)
        {
            try
            {
                PrescriptionViewModel prescription = new PrescriptionViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Prescription/FetchPrescription/Fetch-Prescription?id=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    prescription = JsonConvert.DeserializeObject<PrescriptionViewModel>(data);
                }
                return View(prescription);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }

        [HttpPost]
        public IActionResult EditPrescription(PrescriptionViewModel model, string id)
        {

            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Prescription/UpdatePrescription/Update-Prescription?id=" + id, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = " Prescription successfully updated";
                    return RedirectToAction("PrescriptionsList");
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
        public IActionResult DeletePrescription(string id)
        {
            try
            {
                PrescriptionViewModel prescription = new PrescriptionViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Prescription/FetchPrescription/Fetch-Prescription?id=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    prescription = JsonConvert.DeserializeObject<PrescriptionViewModel>(data);
                }
                return View(prescription);
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
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/Prescription/DeletePrescription/Delete-Prescription?id=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Prescription successfully deleted";
                    return RedirectToAction("PrescriptionsList");
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
