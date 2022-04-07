using HomeFinder.Models;
using HomeFinder.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HomeFinder.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        // GET: /User/Create
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

        // POST: /User/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountModel model)
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



        // GET: /User/Login
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                LoginViewModel model = new LoginViewModel
                {
                    ReturnUrl = returnUrl,
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        // 
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
                                new { ReturnUrl = returnUrl });
            var properties = _signInManager
                .ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult>
            ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: { remoteError}");
                return View("Login", loginViewModel);
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Error loading external login information.");
                return View("Login", loginViewModel);
            }
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, 
                                                                                        info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            FirstName = info.Principal.FindFirstValue(ClaimTypes.Name),
                            LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)
                        };

                        await _userManager.CreateAsync(user);
                    }

                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }
                //TODO: ViewData? Lägg till mail för support
                ViewBag.ErrorTitle = $"Email claim not recived from: {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support at Goat@Goats.bah";

                return View("Error");
                
            }
        }

        // POST: /User/Login
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if(ModelState.IsValid) // Om alla properties validerar med sina attributes:
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                

                if(result.Succeeded) // Om det gick bra att logga in:
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



        // GET: /User/Logout
        [HttpGet]
        public IActionResult Logout() // Om man försöker nå login via url:
        {
            return RedirectToAction("Index", "Home");
        }

        // POST: /User/Logout
        [HttpPost]
        public async Task<IActionResult> Logout(bool? notUsed) // Logout görs med post-request av säkerhetsskäl.
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
            
    }
}
