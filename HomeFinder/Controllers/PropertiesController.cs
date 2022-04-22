using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeFinder.Data;
using HomeFinder.Models;
using HomeFinder.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace HomeFinder.Controllers
{
    [Authorize(Roles = "EstateAgent, Admin")]
    public class PropertiesController : Controller
    {
        private readonly HomeFinderContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PropertiesController(HomeFinderContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Properties
        public async Task<IActionResult> Index()
        {
            var currentUserId = GetUserId();

            // Hämta alla properties
            var properties = await _context.Properties
                .Include(p => p.Adress)
                .Include(p => p.EstateAgent)
                .Include(p => p.PropertyType)
                .Include(p => p.SaleStatus)
                .Include(p => p.Tenure)
                .ToListAsync();

            // Skapa ViewModel med listor för properties och bilder.
            var model = new PropertySummaryViewModel();
            model.Properties = properties;
            model.Images = _context.Images
                .Where(i => i.DisplayImage == true)
                .ToList();

            // Filtrera properties om användren är mäklare.
            if (IsEstateAgent(currentUserId))
            {
                model.Properties = model.Properties
                    .Where(u => u.EstateAgentId == currentUserId)
                    .ToList();
            }

            return View(model);

        }


        // GET: Properties/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
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

            property.NumberOfViews++;
            await _context.SaveChangesAsync();


            PropertyViewModel propertyViewModel = CreatePropertyViewModel();
            propertyViewModel.Property = property;

            // Hämta relevanta bild-url för att skicka in i ViewModel.
            foreach (Image i in _context.Images.ToList())
            {
                if (i.PropertyId == property.Id)
                {
                    propertyViewModel.ImageUrls.Add(i.Url);
                }
            }

            // Visa rätt sak beroende på vem som är inloggad.
            if (User.Identity.Name != null)
            {
                var currentUserId = GetUserId();

                //Kollar om inloggad användare är mäklare, och isåfall om hen är mäklaren för denna bostad.
                if (IsEstateAgent(currentUserId))
                {
                    if (currentUserId != (_context.Properties.FirstOrDefault(p => p.Id == id)).EstateAgentId)
                    {
                        RedirectToAction("AccessDenied", "Account");
                    }
                    else
                    {
                        propertyViewModel.ExpressionsOfInterest = _context.ExpressionOfInterests.ToList();
                    }
                }

                //Annars visar den om inloggad användare har anmält intresse.
                else if (_userManager.IsInRoleAsync(_context.ApplicationUsers.FirstOrDefault(u => u.Id == currentUserId), "User").Result == true)
                {
                    propertyViewModel.ApplicationUser = await _userManager.GetUserAsync(User);

                    ExpressionOfInterest eoi = _context.ExpressionOfInterests.FirstOrDefault(u => u.ApplicationUserId == propertyViewModel.ApplicationUser.Id
                                                             && u.PropertyId == propertyViewModel.Property.Id);
                    if (eoi is not null)
                    {
                        propertyViewModel.IsInterested = true;
                    }
                }
            }

            return View(propertyViewModel);
        }

        // GET: Properties/Create
        public IActionResult Create()
        {
            PropertyViewModel propertyViewModel = CreatePropertyViewModel();

            return View(propertyViewModel);
        }

        // POST: Properties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertyViewModel propertyViewModel)
        {
            Property newProperty = new();

            if (ModelState.IsValid)
            {
                Adress newAdress = propertyViewModel.Adress;
                _context.Adresses.Add(newAdress);
                await _context.SaveChangesAsync();

                newProperty = propertyViewModel.Property;
                newProperty.PublishingDate = DateTime.Now;
                newProperty.NumberOfViews = 0;


                newProperty.Adress = await _context.Adresses.OrderBy(a => a.Id).LastAsync();
                newProperty.AdressId = newProperty.Adress.Id;


                _context.Add(newProperty);
                await _context.SaveChangesAsync();


                // Ladda upp bild
                string uniqueFileName = null;
                if (propertyViewModel.Images != null)
                {
                    foreach (var i in propertyViewModel.Images)
                    {
                        string relativePath = "/images/properties/" + newProperty.Id.ToString();
                        //string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "properties", newProperty.Id.ToString());
                        string uploadsFolder = _hostEnvironment.WebRootPath + relativePath;
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        uniqueFileName = Guid.NewGuid().ToString() + "_" + i.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        i.CopyTo(new FileStream(filePath, FileMode.Create));

                        _context.Images.Add(new Image
                        {
                            Url = Path.Combine(relativePath, uniqueFileName),
                            PropertyId = newProperty.Id
                        });

                        await _context.SaveChangesAsync();
                    }
                    var image = await _context.Images.FirstOrDefaultAsync(i => i.PropertyId == newProperty.Id);
                    if (image != null)
                    {
                        image.DisplayImage = true;
                        await _context.SaveChangesAsync();
                    }
                }


                return RedirectToAction(nameof(Index));

            }
            // TODO: Hantera error om Modelstate inte är valid.

            return View();
        }

        // GET: Properties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var currentUserId = GetUserId();

            if (id == null)
            {
                return NotFound();
            }
            else if (currentUserId != (_context.Properties.FirstOrDefault(p => p.Id == id)).EstateAgentId)
            {
                RedirectToAction("AccessDenied", "Account");
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
        public async Task<IActionResult> Delete(int? id)
        {
            var currentUserId = GetUserId();

            if (id == null)
            {
                return NotFound();
            }
            else if (currentUserId != (_context.Properties.FirstOrDefault(p => p.Id == id)).EstateAgentId)
            {
                RedirectToAction("AccessDenied", "Account");
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


        private PropertyViewModel CreatePropertyViewModel()
        {
            PropertyViewModel propertyViewModel = new();

            propertyViewModel.SaleStatuses = _context.SaleStatuses.ToList();
            propertyViewModel.Tenures = _context.Tenures.ToList();
            propertyViewModel.PropertyTypes = _context.PropertyTypes.ToList();

            propertyViewModel.Users = _context.ApplicationUsers.ToList();

            foreach (var u in propertyViewModel.Users)
            {
                if (_userManager.IsInRoleAsync(u, "EstateAgent").Result)
                {
                    propertyViewModel.EstateAgents.Add(u);
                }
            }


            return propertyViewModel;

        }


        private string GetUserId()
        {
            var currentUser = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name);
            return currentUser.Id;
        }

        private bool IsEstateAgent(string userid)
        {
            var currentUser = _context.ApplicationUsers.FirstOrDefault(u => u.Id == userid);


            if (_userManager.IsInRoleAsync(currentUser, "EstateAgent").Result == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}