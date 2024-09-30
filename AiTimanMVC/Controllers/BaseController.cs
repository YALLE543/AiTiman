using Microsoft.AspNetCore.Mvc;

namespace AiTimanMVC.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
