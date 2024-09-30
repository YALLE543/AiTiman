using Microsoft.AspNetCore.Mvc;

namespace AiTimanMVC.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BookedAppointments()
        {
            return View();
        }
    }
}
