using HomeFinder.Data;
using HomeFinder.Models;
using HomeFinder.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HomeFinder.Controllers
{
    public class EstateAgentOptions : Controller
    {

        private readonly HomeFinderContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EstateAgentOptions(HomeFinderContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowProperties(string userid)
{
            var properties = _context.Properties.Include(p => p.Adress).Include(p => p.EstateAgent).Include(p => p.PropertyType).Include(p => p.SaleStatus).Include(p => p.Tenure);
            var userProperties = properties.Where(u=>u.EstateAgentId == userid);

            return View(userProperties.ToList());
        }



        // GET: Properties/Details/5
        public async Task<IActionResult> Details(int? id, string userid)
        {
            if (id == null)
            {
                return NotFound();
            }

            else if (userid != (_context.Properties.FirstOrDefault(p => p.Id == id)).EstateAgentId)
            {
                //TODO: Ändra till access denied
                return NotFound();
            }

            var property = await _context.Properties
                .Include(p => p.Adress)
                .Include(p => p.EstateAgent)
                .Include(p => p.PropertyType)
                .Include(p => p.SaleStatus)
                .Include(p => p.Tenure)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (property == null)
            {
                return NotFound();
            }
            PropertyViewModel propertyViewModel = new();
            propertyViewModel.Property = property;
            propertyViewModel.ExpressionsOfInterest = _context.ExpressionOfInterests.ToList();

            return View(propertyViewModel);
        }


        // GET: Properties/Edit/5
        public async Task<IActionResult> Edit(int? id, string userid)
        {
            if (id == null)
            {
                return NotFound();
            }

            else if (userid != (_context.Properties.FirstOrDefault(p => p.Id == id)).EstateAgentId)
            {
                //TODO: Ändra till access denied
                return NotFound();
            }

            PropertyViewModel propertyViewModel = CreatePropertyViewModel();
            Property property = await _context.Properties.FindAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            propertyViewModel.Property = property;
            propertyViewModel.Adress = await _context.Adresses.FindAsync(property.AdressId);

            return View(propertyViewModel);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PropertyViewModel propertyViewModel)
        {
            if (id != propertyViewModel.Property.Id)
            {
                return NotFound();
            }

            Property property = new();

            if (ModelState.IsValid)
            {
                try
                {
                    property = propertyViewModel.Property;

                    Adress adress = _context.Adresses.FirstOrDefault(a => a.Street == propertyViewModel.Adress.Street &&
                                                                    a.StreetNumber == propertyViewModel.Adress.StreetNumber &&
                                                                    a.PostalCode == propertyViewModel.Adress.PostalCode &&
                                                                    a.City == propertyViewModel.Adress.City &&
                                                                    a.Country == propertyViewModel.Adress.Country);

                    if (adress != null)
                    {
                        property.AdressId = adress.Id;
                    }
                    else
                    {
                        Adress newAdress = propertyViewModel.Adress;
                        _context.Adresses.Add(newAdress);
                        await _context.SaveChangesAsync();

                        property.Adress = await _context.Adresses.OrderBy(a => a.Id).LastAsync();
                        property.AdressId = property.Adress.Id;
                    }




                    _context.Update(property);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(property.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View();
        }


        // GET: Properties/Delete/5
        public async Task<IActionResult> Delete(int? id, string userid)
        {
            if (id == null)
            {
                return NotFound();
            }

            else if (userid != (_context.Properties.FirstOrDefault(p => p.Id == id)).EstateAgentId)
            {
                //TODO: Ändra till access denied
                return NotFound();
            }

            var property = await _context.Properties
                .Include(p => p.Adress)
                .Include(p => p.EstateAgent)
                .Include(p => p.PropertyType)
                .Include(p => p.SaleStatus)
                .Include(p => p.Tenure)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyExists(int id)
        {
            return _context.Properties.Any(e => e.Id == id);
        }


        public PropertyViewModel CreatePropertyViewModel()
        {
            PropertyViewModel propertyViewModel = new();

            propertyViewModel.SaleStatuses = _context.SaleStatuses.Where(a => a.Description != null).ToList();
            propertyViewModel.Tenures = _context.Tenures.Where(a => a.Description != null).ToList();
            propertyViewModel.PropertyTypes = _context.PropertyTypes.Where(a => a.Description != null).ToList();

//TODO: Lägg till att bara mäklare ska dyka upp i listan nedan!
            propertyViewModel.EstateAgents = _userManager.Users.Where(a => a.UserName != null).ToList();


            return propertyViewModel;

        }


    }
}
