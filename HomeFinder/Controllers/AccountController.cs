using HomeFinder.Models;
using HomeFinder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HomeFinder.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }



        // GET: /Account/Create
        [HttpGet]
        public IActionResult Create()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: /Account/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountModel model)
        {
            if (ModelState.IsValid) // Om alla properties validerar med sina attributes:
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
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, model.RememberMe);
                    return RedirectToAction("Index", "Home");
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



        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: /Account/Login
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid) // Om alla properties validerar med sina attributes:
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);


                if (result.Succeeded) // Om det gick bra att logga in:
                {
                    if ((!string.IsNullOrEmpty(returnUrl))
                        && (Url.IsLocalUrl(returnUrl))) // (säkerhetskontroll)
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else // Om det inte gick att logga in:
                {
                    ModelState.AddModelError("", "Login failed.");
                }
            }

            return View(model);
        }



        // GET: /Account/Logout
        [HttpGet]
        [Authorize]
        public IActionResult Logout() // Om man försöker nå login via url:
        {
            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout(bool? notUsed) // Logout görs med post-request av säkerhetsskäl.
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }
    }
}