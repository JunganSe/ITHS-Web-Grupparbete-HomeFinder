using Microsoft.AspNetCore.Mvc;

namespace HomeFinder.Controllers
{
    public class AdminOptions : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
