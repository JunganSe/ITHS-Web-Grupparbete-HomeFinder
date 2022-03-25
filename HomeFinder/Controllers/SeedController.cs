using HomeFinder.Data;
using HomeFinder.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeFinder.Controllers
{
    public class SeedController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HomeFinderContext _context;
        public SeedController(HomeFinderContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult SeedData()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SeedData(bool? notUsed)
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.MigrateAsync();

            List<Adress> adresses = new()
            {
                new Adress()
                {
                    Street = "Testvägen",
                    StreetNumber = 2,
                    PostalCode = "12345",
                    City = "Testtown",
                    Country = "Testland"
                },
                new Adress()
                {
                    Street = "Provgatan",
                    StreetNumber = 4,
                    PostalCode = "54321",
                    City = "Provtown",
                    Country = "Provland"
                },
                new Adress()
                {
                    Street = "Getvägen",
                    StreetNumber = 21,
                    PostalCode = "33212",
                    City = "Jönköping",
                    Country = "Sverige"
                },
                new Adress()
                {
                    Street = "Vegen",
                    StreetNumber = 41,
                    PostalCode = "43521",
                    City = "Køpenhavn",
                    Country = "Dnmark"
                }
            };
            await _context.Adresses.AddRangeAsync(adresses);

            var applicationUser = new ApplicationUser
            {
                FirstName = "Testarn",
                LastName = "Testarnsson",
                Email = "test@test.com",
                UserName = "test@test.com"
            };
            string password = "Test123!";
            await _userManager.CreateAsync(applicationUser, password);

            var applicationUser1 = new ApplicationUser
            {
                FirstName = "Tommy",
                LastName = "Tomtefar",
                Email = "Tompa@tompasventilation.se",
                UserName = "Tompa@tompasventilation.se"
            };
            string password1 = "Test123!";
            await _userManager.CreateAsync(applicationUser1, password1);

            List<SaleStatus> saleStatuses = new()
            {
                new SaleStatus { Description = "For sale"},
                new SaleStatus { Description = "Not for sale"},
                new SaleStatus { Description = "Sold"}
            };
            await _context.SaleStatuses.AddRangeAsync(saleStatuses);

            List<Tenure> tenures = new()
            {
                new Tenure { Description = "For sale"},
                new Tenure { Description = "For rent" },
                new Tenure { Description = "Condominium" }
            };
            await _context.Tenures.AddRangeAsync(tenures);

            List<PropertyType> propertyTypes = new()
            {
                new PropertyType { Description = "Villa" },
                new PropertyType { Description = "Apartment" },
                new PropertyType { Description = "Terrace house" },
                new PropertyType { Description = "Farm" },
                new PropertyType { Description = "Plot" },
                new PropertyType { Description = "Husvilla" }
            };
            await _context.PropertyTypes.AddRangeAsync(propertyTypes);
            await _context.SaveChangesAsync();

            List<Property> properties = new()
            {
                new Property()
                {
                    Price = 10m,
                    Description = "Enrumsvilla med stort kök för storkok av löksås. Stort träd i mitten av huset ger en närhet till naturen.",
                    Summary = "Fräsig sekelskiftesvilla i Skellefteås innerstad",
                    NumberOfRooms = 1,
                    BuildingArea = 100.2,
                    PlotArea = 100.3,
                    ConstructionYear = 1502,
                    MapUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d5869471.5021478925!2d58.41210746720272!3d42.32264009498601!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x995de1c8a9bc792d!2zNDLCsDIwJzQ0LjAiTiA2MsKwMzInNDkuMiJF!5e0!3m2!1sen!2sse!4v1648042744494!5m2!1sen!2sse",
                    PublishingDate = DateTime.Now,
                    ViewingDate = DateTime.Now,
                    Adress = await _context.Adresses.FirstOrDefaultAsync(a => a.City != null),
                    PropertyType = await _context.PropertyTypes.FirstOrDefaultAsync(p => p.Description != null),
                    Tenure = await _context.Tenures.FirstOrDefaultAsync(t => t.Description != null),
                    SaleStatus = await _context.SaleStatuses.FirstOrDefaultAsync(s => s.Description != null),
                    EstateAgent = await _context.ApplicationUsers.FirstOrDefaultAsync(a => a.FirstName != null)
                },
                new Property()
                {
                    Price = 63000000m,
                    Description = "Mysig platseffektiv lägenhet i stockholmsinnerstad. Toalett saknas med i området finns många restauranger. Nära Spybar.",
                    Summary = "Lägenhet för folk med känd instagram.",
                    NumberOfRooms = 1,
                    BuildingArea = 13,
                    PlotArea = 0,
                    ConstructionYear = 2021,
                    MapUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d5869471.5021478925!2d58.41210746720272!3d42.32264009498601!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x995de1c8a9bc792d!2zNDLCsDIwJzQ0LjAiTiA2MsKwMzInNDkuMiJF!5e0!3m2!1sen!2sse!4v1648042744494!5m2!1sen!2sse",
                    PublishingDate = DateTime.Now,
                    ViewingDate = DateTime.Now,
                    Adress = await _context.Adresses.FindAsync(3),
                    PropertyType = await _context.PropertyTypes.FindAsync(2),
                    Tenure = await _context.Tenures.FindAsync(3),
                    SaleStatus = await _context.SaleStatuses.FindAsync(1),
                    EstateAgent = await _context.ApplicationUsers.FirstOrDefaultAsync(a => a.FirstName != null)
                },
                new Property()
                {
                    Price = 10000000m,
                    Description = "Stuga. I skogen.",
                    Summary = "Fin.",
                    NumberOfRooms = 6,
                    BuildingArea = 120,
                    PlotArea = 1200,
                    ConstructionYear = 1852,
                    MapUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d5869471.5021478925!2d58.41210746720272!3d42.32264009498601!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x995de1c8a9bc792d!2zNDLCsDIwJzQ0LjAiTiA2MsKwMzInNDkuMiJF!5e0!3m2!1sen!2sse!4v1648042744494!5m2!1sen!2sse",
                    PublishingDate = DateTime.Now,
                    ViewingDate = DateTime.Now,
                    Adress = await _context.Adresses.FindAsync(2),
                    PropertyType = await _context.PropertyTypes.FindAsync(6),
                    Tenure = await _context.Tenures.FindAsync(1),
                    SaleStatus = await _context.SaleStatuses.FindAsync(1),
                    EstateAgent = await _context.ApplicationUsers.FirstOrDefaultAsync(a => a.FirstName != null)
                },
            };
            
            await _context.Properties.AddRangeAsync(properties);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
