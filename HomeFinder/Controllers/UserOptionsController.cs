using Microsoft.AspNetCore.Mvc;
using HomeFinder.ViewModels;
using Microsoft.AspNetCore.Identity;
using HomeFinder.Models;
using System.Threading.Tasks;
using HomeFinder.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HomeFinder.Controllers
{
    public class UserOptionsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HomeFinderContext _context;

        public UserOptionsController(UserManager<ApplicationUser> userManager, HomeFinderContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: /UserOptions/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }



        // GET: /UserOptions/UpdateProfile
        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            // Hämta data från databasen, lägg den i model och skicka till view.
            var user = await _userManager.GetUserAsync(User);
            var model = new UserOptionsViewModel()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return View(model);
        }

        // POST: /UserOptions/UpdateProfile
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserOptionsViewModel model)
        {
            var user = await _userManager.GetUserAsync(User); // Hämta den inloggade användaren.
            if(ModelState.IsValid)
            {
                // Om lösenordet har fyllts i korrekt:
                if (await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    if (model.Email == user.Email && model.FirstName == user.FirstName && model.LastName == user.LastName &&
                        string.IsNullOrEmpty(model.NewPassword))
                    {

                        ViewData["Message"] = "No Changes";
                        return View();
                    }
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    ViewData["Message"] = "Yur stuuf is updaet.";

                    // Hämta uppgifter från formuläret och lägg dem i användarens data.
                    if (!string.IsNullOrEmpty(model.NewPassword)) // Om ett nytt lösenord har fyllts i:
                    {
                        //kontrollerar att nya lösenordet uppfyller kraven
                        var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
                        if (result.Succeeded == true)
                        {
                            ViewData["Message"] += " Paiswor is updaet.";
                        }
                        else
                        {
                            ViewData["Message"] = "New Password is WEAK. Just like you.";
                            return View();                           
                        }
                    }                   
                    await _userManager.UpdateAsync(user); // Skicka in ändringarna i databasen.
                }
                else
                {
                    ViewData["Message"] = "Wrong password.";
                }
            }
            else
            {
                ViewData["Message"] = "Incorrect Input";
            }
            return View();
        }



        // GET: /UserController/ManageExpressionOfInterests
        [HttpGet]
        public async Task<IActionResult> DisplayExpressionOfInterests()
        {
            var user = await _userManager.GetUserAsync(User);

            // Troligtvis överkomplicerat sätt att hämta möjliga värden för SaleStatus ur databasen och skicka dem till view i en dictionary.
            var ids = _context.SaleStatuses
                .Select(s => s.Id)
                .ToList();
            var descriptions = _context.SaleStatuses
                .Select(s => s.Description)
                .ToList();
            Dictionary<string, int> saleStatuses = new();
            for (int i = 0; i < ids.Count; i++)
            {
                saleStatuses.Add(descriptions[i], ids[i]);
            }

            // Hämta en lista med id på de properties som användaren intresseanmält.
            var propertyIds = _context.ExpressionOfInterests // Titta i kopplingstabellen.
                .Where(e => e.ApplicationUserId == user.Id) // Ta bara de rader med rätt användar-id.
                .Select(e => e.PropertyId) // Hämta property-id från de raderna.
                .ToList();
            
            var model = new DisplayExpressionOfInterestsViewModel()
            {
                SaleStatuses = saleStatuses,

                // Hämta alla properties vars id finns i propertyIds-listan.
                Properties = _context.Properties
                    .Include(p => p.Adress)
                    .ToList()
                    .FindAll(p => propertyIds
                        .Contains(p.Id))
            };

            return View(model);
        }
    }
}
