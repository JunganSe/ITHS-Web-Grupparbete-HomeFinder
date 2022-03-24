using HomeFinder.Models;
using HomeFinder.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HomeFinder.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        // GET: /User/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /User/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if(ModelState.IsValid) // Om alla properties validerar med sina attributes:
            {
                var user = new ApplicationUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded) // Om det gick bra att skapa användaren:
                {
                    await _signInManager.SignInAsync(user, model.RememberMe);
                    return RedirectToAction("Index", "Home"); // TODO: Ändra till nåt vettigt.
                }
                else // Om det inte gick att skapa användaren:
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }



        // GET: /User/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /User/Login
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if(ModelState.IsValid) // Om alla properties validerar med sina attributes:
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if(result.Succeeded) // Om det gick bra att logga in:
                {
                    if ((!string.IsNullOrEmpty(returnUrl)) 
                        && (Url.IsLocalUrl(returnUrl))) // (säkerhetskontroll)
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home"); // TODO: Ändra till nåt vettigt.
                    }
                }
                else // Om det inte gick att logga in:
                {
                    ModelState.AddModelError("", "Login failed.");
                }
            }

            return View(model);
        }



        // POST: /User/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        // GET: /User/Settings
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            return Ok("User settings page goes here.");
        }
            
    }
}
