using HomeFinder.Data;
using HomeFinder.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeFinder.Controllers
{
    public class SeedController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly HomeFinderContext _context;

        public SeedController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                RoleManager<IdentityRole> roleManager,
                                HomeFinderContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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

            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedAssignRolesAsync();
            await SeedSaleStatusesAsync();
            await SeedTenuresAsync();
            await SeedPropertyTypesAsync();
            await SeedAdressesAsync();
            await _context.SaveChangesAsync();
            await SeedPropertiesAsync();
            await _context.SaveChangesAsync();
            await SeedImagesAsync();
            await SeedExpressionOfInterestsAsync();

            await _context.SaveChangesAsync();
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        private async Task SeedRolesAsync()
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = "User" });
            await _roleManager.CreateAsync(new IdentityRole { Name = "EstateAgent" });
            await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
        }

        private async Task SeedUsersAsync()
        {
            string password = "Test123!";

            var applicationUser1 = new ApplicationUser
            {
                FirstName = "Testarn",
                LastName = "Testarnsson (användare)",
                Email = "test@test",
                UserName = "test@test"
            };
            await _userManager.CreateAsync(applicationUser1, password);

            var applicationUser2 = new ApplicationUser
            {
                FirstName = "Micke",
                LastName = "Med kaffet (mäklare)",
                Email = "micke@kaffe",
                UserName = "micke@kaffe"
            };
            await _userManager.CreateAsync(applicationUser2, password);

            var applicationUser3 = new ApplicationUser
            {
                FirstName = "Goatman",
                LastName = "Asskickingson (admin)",
                Email = "admin@admin",
                UserName = "admin@admin"
            };
            await _userManager.CreateAsync(applicationUser3, password);

            var applicationUser4 = new ApplicationUser
            {
                FirstName = "Tommy",
                LastName = "Tomtefar",
                Email = "Tompa@tompasventilation.se",
                UserName = "Tompa@tompasventilation.se"
            };
            await _userManager.CreateAsync(applicationUser4, password);

            var applicationUser5 = new ApplicationUser
            {
                FirstName = "Roy",
                LastName = "på Macken (mäklare)",
                Email = "roy@macken.se",
                UserName = "roy@macken.se"
            };
            await _userManager.CreateAsync(applicationUser5, password);

            var applicationUser6 = new ApplicationUser
            {
                FirstName = "Klas",
                LastName = "Kalas (användare)",
                Email = "klaws@google.com",
                UserName = "klaws@google.com"
            };
            await _userManager.CreateAsync(applicationUser6, password);
        }

        private async Task SeedAssignRolesAsync()
        {
            var user1 = await _context.ApplicationUsers.FirstAsync(a => a.UserName == "test@test");
            await _userManager.AddToRoleAsync(user1, "User");

            var user2 = await _context.ApplicationUsers.FirstAsync(a => a.UserName == "micke@kaffe");
            await _userManager.AddToRoleAsync(user2, "EstateAgent");

            var user3 = await _context.ApplicationUsers.FirstAsync(a => a.UserName == "admin@admin");
            await _userManager.AddToRoleAsync(user3, "Admin");

            var user4 = await _context.ApplicationUsers.FirstAsync(a => a.UserName == "roy@macken.se");
            await _userManager.AddToRoleAsync(user4, "EstateAgent");

            var user5 = await _context.ApplicationUsers.FirstAsync(a => a.UserName == "klaws@google.com");
            await _userManager.AddToRoleAsync(user5, "User");
        }

        private async Task SeedSaleStatusesAsync()
        {
            List<SaleStatus> saleStatuses = new()
            {
                new SaleStatus { Description = "Sold" },
                new SaleStatus { Description = "For sale" },
                new SaleStatus { Description = "Not for sale" }
            };
            await _context.SaleStatuses.AddRangeAsync(saleStatuses);
        }

        private async Task SeedTenuresAsync()
        {
            List<Tenure> tenures = new()
            {
                new Tenure { Description = "Tenancy" },
                new Tenure { Description = "Condominium" },
                new Tenure { Description = "Owner occupancy" }
            };
            await _context.Tenures.AddRangeAsync(tenures);
        }

        private async Task SeedPropertyTypesAsync()
        {
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
        }

        private async Task SeedAdressesAsync()
        {
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
                },
                new Adress()
                {
                    Street = "Kyrkan",
                    StreetNumber = 22,
                    PostalCode = "53461",
                    City = "Vedum",
                    Country = "Sverige"
                },
                new Adress()
                {
                    Street = "Hajvägen",
                    StreetNumber = 451,
                    PostalCode = "22165",
                    City = "Treriksröset",
                    Country = "Finland"
                }
            };
            await _context.Adresses.AddRangeAsync(adresses);
        }

        private async Task SeedPropertiesAsync()
        {
            List<Property> properties = new()
            {
                new Property()
                {
                    Price = 10m,
                    Description = "Enrumsvilla med stort kök för storkok av löksås. Stort träd i mitten av huset ger en närhet till naturen.",
                    Summary = "Fräsig sekelskiftesvilla i Skellefteås innerstad",
                    NumberOfRooms = 1,
                    BuildingArea = 100.2,
                    BeeArea = 15,
                    PlotArea = 100.3,
                    ConstructionYear = 1502,
                    MapUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d5869471.5021478925!2d58.41210746720272!3d42.32264009498601!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x995de1c8a9bc792d!2zNDLCsDIwJzQ0LjAiTiA2MsKwMzInNDkuMiJF!5e0!3m2!1sen!2sse!4v1648042744494!5m2!1sen!2sse",
                    PublishingDate = DateTime.Now,
                    ViewingDate = DateTime.Now,
                    Adress = await _context.Adresses.FindAsync(1),
                    PropertyType = await _context.PropertyTypes.FindAsync(2),
                    Tenure = await _context.Tenures.FindAsync(2),
                    SaleStatus = await _context.SaleStatuses.FindAsync(2),
                    EstateAgent = await _context.ApplicationUsers.FirstAsync(a => a.UserName == "micke@kaffe"),
                    NumberOfViews = 60
                },
                new Property()
                {
                    Price = 63000000m,
                    Description = "Mysig platseffektiv lägenhet i stockholmsinnerstad. Toalett saknas med i området finns många restauranger. Nära Spybar.",
                    Summary = "Lägenhet för folk med känd instagram.",
                    NumberOfRooms = 1,
                    BuildingArea = 13,
                    BeeArea = 150,
                    PlotArea = 0,
                    ConstructionYear = 2021,
                    MapUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d5869471.5021478925!2d58.41210746720272!3d42.32264009498601!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x995de1c8a9bc792d!2zNDLCsDIwJzQ0LjAiTiA2MsKwMzInNDkuMiJF!5e0!3m2!1sen!2sse!4v1648042744494!5m2!1sen!2sse",
                    PublishingDate = DateTime.Now,
                    ViewingDate = DateTime.Now,
                    Adress = await _context.Adresses.FindAsync(2),
                    PropertyType = await _context.PropertyTypes.FindAsync(2),
                    Tenure = await _context.Tenures.FindAsync(3),
                    SaleStatus = await _context.SaleStatuses.FindAsync(2),
                    EstateAgent = await _context.ApplicationUsers.FirstAsync(a => a.UserName == "micke@kaffe"),
                    NumberOfViews = 95
                },
                new Property()
                {
                    Price = 10000000m,
                    Description = "Stuga. I skogen. Träd kan finnas i närområdet.",
                    Summary = "Fin.",
                    NumberOfRooms = 6,
                    BuildingArea = 120,
                    BeeArea = 8,
                    PlotArea = 1200,
                    ConstructionYear = 1852,
                    MapUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d5869471.5021478925!2d58.41210746720272!3d42.32264009498601!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x995de1c8a9bc792d!2zNDLCsDIwJzQ0LjAiTiA2MsKwMzInNDkuMiJF!5e0!3m2!1sen!2sse!4v1648042744494!5m2!1sen!2sse",
                    PublishingDate = DateTime.Now,
                    ViewingDate = DateTime.Now,
                    Adress = await _context.Adresses.FindAsync(3),
                    PropertyType = await _context.PropertyTypes.FindAsync(6),
                    Tenure = await _context.Tenures.FindAsync(1),
                    SaleStatus = await _context.SaleStatuses.FindAsync(2),
                    EstateAgent = await _context.ApplicationUsers.FirstAsync(a => a.UserName == "micke@kaffe")
                },
                new Property()
                {
                    Price = 10000000m,
                    Description = "Koja på taket med soltak och kompost i sovrummet.",
                    Summary = "Casa de Karlsson.",
                    NumberOfRooms = 1,
                    BuildingArea = 12,
                    BeeArea = 5000,
                    PlotArea = 200000,
                    ConstructionYear = 1949,
                    MapUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d5869471.5021478925!2d58.41210746720272!3d42.32264009498601!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x995de1c8a9bc792d!2zNDLCsDIwJzQ0LjAiTiA2MsKwMzInNDkuMiJF!5e0!3m2!1sen!2sse!4v1648042744494!5m2!1sen!2sse",
                    PublishingDate = DateTime.Now,
                    ViewingDate = DateTime.Now,
                    Adress = await _context.Adresses.FindAsync(4),
                    PropertyType = await _context.PropertyTypes.FindAsync(2),
                    Tenure = await _context.Tenures.FindAsync(2),
                    SaleStatus = await _context.SaleStatuses.FindAsync(3),
                    EstateAgent = await _context.ApplicationUsers.FirstAsync(a => a.UserName == "micke@kaffe"),
                    NumberOfViews = 3
                },
                new Property()
                {
                    Price = 66m,
                    Description = "Biluthyrningslokaler komplett med rostiga bilar att pilla med.",
                    Summary = "Roy och Rogers bilservice",
                    NumberOfRooms = 2,
                    BuildingArea = 150,
                    BeeArea = 400,
                    PlotArea = 700,
                    ConstructionYear = 1986,
                    MapUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d8423.50112108746!2d12.976587394679703!3d58.14223879867664!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x465ac893468b1733%3A0xb2b98809eb817ad9!2s534%2061%20Bitterna!5e0!3m2!1ssv!2sse!4v1649322794451!5m2!1ssv!2sse",
                    PublishingDate = DateTime.Now,
                    ViewingDate = DateTime.Now,
                    Adress = await _context.Adresses.FindAsync(5),
                    PropertyType = await _context.PropertyTypes.FindAsync(5),
                    Tenure = await _context.Tenures.FindAsync(2),
                    SaleStatus = await _context.SaleStatuses.FindAsync(2),
                    EstateAgent = await _context.ApplicationUsers.FirstAsync(a => a.UserName == "roy@macken.se")
                },
                new Property()
                {
                    Price = 200000m,
                    Description = "Vill du äga tre länder på samma gång? Självklart vill du det.",
                    Summary = "Ultimat lyx.",
                    NumberOfRooms = 1,
                    BuildingArea = 150,
                    BeeArea = 42,
                    PlotArea = 89000,
                    ConstructionYear = 1101,
                    MapUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d19032257.77307806!2d-3.9812940566744657!3d54.38748127492599!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x45daaf37376e3d75%3A0x35cbf09316b8d2eb!2sTreriksr%C3%B6set!5e0!3m2!1ssv!2sse!4v1649323194113!5m2!1ssv!2sse",
                    PublishingDate = DateTime.Now,
                    ViewingDate = DateTime.Now,
                    Adress = await _context.Adresses.FindAsync(6),
                    PropertyType = await _context.PropertyTypes.FindAsync(5),
                    Tenure = await _context.Tenures.FindAsync(3),
                    SaleStatus = await _context.SaleStatuses.FindAsync(2),
                    EstateAgent = await _context.ApplicationUsers.FirstAsync(a => a.UserName == "roy@macken.se"),
                    NumberOfViews = 10000
                }
            };
            await _context.Properties.AddRangeAsync(properties);
        }

        private async Task SeedImagesAsync()
        {
            var properties = await _context.Properties.ToListAsync();

            List<Image> images = new()
            {
                new Image()
                {
                    Url = "/images/properties/default/default-house-1.png",
                    Property = properties[0],
                    DisplayImage = true
                },
                new Image()
                {
                    Url = "/images/properties/default/default-house-interior-1.png",
                    Property = properties[0]
                },
                new Image()
                {
                    Url = "/images/properties/default/default-house-interior-2.png",
                    Property = properties[0]
                },

                new Image()
                {
                    Url = "/images/properties/default/default-house-2.png",
                    Property = properties[1],
                    DisplayImage = true
                },
                new Image()
                {
                    Url = "/images/properties/default/default-house-interior-2.png",
                    Property = properties[1]
                },
                new Image()
                {
                    Url = "/images/properties/default/default-house-interior-3.png",
                    Property = properties[1]
                },

                new Image()
                {
                    Url = "/images/properties/default/default-house-3.png",
                    Property = properties[2],
                    DisplayImage = true
                },
                new Image()
                {
                    Url = "/images/properties/default/default-house-interior-3.png",
                    Property = properties[2]
                },
                new Image()
                {
                    Url = "/images/properties/default/default-house-interior-4.png",
                    Property = properties[2]
                },

                new Image()
                {
                    Url = "/images/properties/default/default-house-4.png",
                    Property = properties[3],
                    DisplayImage = true
                },
                new Image()
                {
                    Url = "/images/properties/default/default-house-interior-4.png",
                    Property = properties[3]
                },
                new Image()
                {
                    Url = "/images/properties/default/default-house-interior-1.png",
                    Property = properties[3]
                },

                new Image()
                {
                    Url = "/images/properties/default/default-house-1.png",
                    Property = properties[4],
                    DisplayImage = true
                },
                new Image()
                {
                    Url = "/images/properties/default/default-house-interior-2.png",
                    Property = properties[4]
                },
                new Image()
                {
                    Url = "/images/properties/default/default-house-interior-3.png",
                    Property = properties[4]
                },

                new Image()
                {
                    Url = "/images/properties/default/default-house-2.png",
                    Property = properties[5],
                    DisplayImage = true
                },
                new Image()
                {
                    Url = "/images/properties/default/default-house-interior-3.png",
                    Property = properties[5]
                },
                new Image()
                {
                    Url = "/images/properties/default/default-house-interior-1.png",
                    Property = properties[5]
                }
            };
            await _context.Images.AddRangeAsync(images);
        }

        private async Task SeedExpressionOfInterestsAsync()
        {
            List<ExpressionOfInterest> expressionOfInterests = new()
            {
                new ExpressionOfInterest()
                {
                    ApplicationUser = await _context.ApplicationUsers.FirstAsync(a => a.UserName == "test@test"),
                    Property = await _context.Properties.FindAsync(1)
                },
                new ExpressionOfInterest()
                {
                    ApplicationUser = await _context.ApplicationUsers.FirstAsync(a => a.UserName == "test@test"),
                    Property = await _context.Properties.FindAsync(2)
                },
                new ExpressionOfInterest()
                {
                    ApplicationUser = await _context.ApplicationUsers.FirstAsync(a => a.UserName == "test@test"),
                    Property = await _context.Properties.FindAsync(3)
                },
                new ExpressionOfInterest()
                {
                    ApplicationUser = await _context.ApplicationUsers.FirstAsync(a => a.UserName == "klaws@google.com"),
                    Property = await _context.Properties.FindAsync(6)
                },
                new ExpressionOfInterest()
                {
                    ApplicationUser = await _context.ApplicationUsers.FirstAsync(a => a.UserName == "klaws@google.com"),
                    Property = await _context.Properties.FindAsync(5)
                },
                new ExpressionOfInterest()
                {
                    ApplicationUser = await _context.ApplicationUsers.FirstAsync(a => a.UserName == "test@test"),
                    Property = await _context.Properties.FindAsync(5)
                }

            };
            await _context.ExpressionOfInterests.AddRangeAsync(expressionOfInterests);
        }
    }
}