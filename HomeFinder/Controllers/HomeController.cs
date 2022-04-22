using HomeFinder.Data;
using HomeFinder.Models;
using HomeFinder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HomeFinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HomeFinderContext _context;

        public HomeController(ILogger<HomeController> logger, HomeFinderContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new();
            var properties = _context.Properties.Include(p => p.Adress).Include(p => p.Tenure).Include(p => p.PropertyType).ToList();
            var sortPropViews = properties.OrderByDescending(p => p.NumberOfViews).ToList();
            var sortPropSize = properties.OrderByDescending(p => p.BuildingArea + p.BeeArea).ToList();
            var sortPrice = properties.OrderByDescending(p => p.Price).ToList();
            var images = _context.Images.Where(p => p.DisplayImage == true).ToList();

            var top3ListViews = new List<Property>();
            var top3ListSize = new List<Property>();
            var top3ListPrice = new List<Property>();

            for (int i = 0; i < 3; i++)
            {
                top3ListViews.Add(sortPropViews[i]);
                top3ListSize.Add(sortPropSize[i]);
                top3ListPrice.Add(sortPrice[i]);
            }
            homeViewModel.Images = images;
            homeViewModel.Top3Price = top3ListPrice;
            homeViewModel.Top3Size = top3ListSize;
            homeViewModel.Top3Viewed = top3ListViews;
            return View(homeViewModel);
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return Redirect("https://youtu.be/j5a0jTc9S10");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
