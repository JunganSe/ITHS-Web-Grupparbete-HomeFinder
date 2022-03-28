using Microsoft.AspNetCore.Mvc;
using HomeFinder.ViewModels;
using Microsoft.AspNetCore.Identity;
using HomeFinder.Models;
using System.Threading.Tasks;

namespace HomeFinder.Controllers
{
    public class UserOptionsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserOptionsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /UserOptions/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new UserOptionsViewModel()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserOptionsViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if(ModelState.IsValid)
            {
                if ((!string.IsNullOrEmpty(model.Password))
                    && (await _userManager.CheckPasswordAsync(user, model.Password)))
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    if (!string.IsNullOrEmpty(model.NewPassword))
                    {
                        await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
                    }
                    ViewData["Message"] = "Yur stuuf is updaet.";
                }
                else
                {
                    ViewData["Message"] = "Wrong password.";
                }
                await _userManager.UpdateAsync(user);
            }
            else
            {
                ViewData["Message"] = "Incorrect input.";
            }

            return View("Index");
        }
    }
}
