using Microsoft.AspNetCore.Mvc;

namespace HomeFinder.Controllers
{
    public class UserOptions : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
