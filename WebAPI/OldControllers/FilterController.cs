using HomeFinder.Data;
using HomeFinder.Models;
using HomeFinder.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeFinder.Controllers
{
    public class FilterController : Controller
    {
        private readonly HomeFinderContext _context;

        public FilterController(HomeFinderContext context)
        {
            _context = context;
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Search(int minRooms, string city, int maxRooms, int selectedPropertyType, string zipCode, int minArea, int maxArea,
            string country, int minPrice, int maxPrice, int selectedTenureType)
        {
            FilterViewModel filterViewModel = new();
            var properties = _context.Properties.Include(p => p.Address).ToList();
            var propertyTypes = _context.PropertyTypes.Select(pt => pt).ToList();
            var tenures = _context.Tenures.Select(t => t).ToList();
            filterViewModel.Tenures = tenures;
            filterViewModel.PropertyTypes = propertyTypes;
            if (!string.IsNullOrEmpty(city))
            {
                properties = properties.Where(p => p.Address.City.Contains(city)).ToList();

            }
            if (minRooms != 0)
            {
                properties = properties.Where(p => p.NumberOfRooms >= minRooms).ToList();

            }
            if (maxRooms != 0)
            {

                properties = properties.Where(p => p.NumberOfRooms <= maxRooms).ToList();

            }
            if (!string.IsNullOrEmpty(zipCode))
            {

                properties = properties.Where(p => p.Address.PostalCode == zipCode).ToList();

            }
            if (!string.IsNullOrEmpty(country))
            {

                properties = properties.Where(p => p.Address.Country == country).ToList();

            }
            if (selectedPropertyType != 0)
            {
                properties = properties.Where(p => p.PropertyType.Id == selectedPropertyType).ToList();
            }
            if (selectedTenureType != 0)
            {
                properties = properties.Where(p => p.Tenure.Id == selectedTenureType).ToList();
            }
            //if(!string.IsNullOrEmpty(minArea))
            //{
            //    var min = int.Parse(minArea);
            //    properties = properties.Where(p => p.PlotArea >= min).ToList();
            //}
            if (minArea != 0)
            {
                properties = properties.Where(p => p.PlotArea >= minArea).ToList();
            }
            if (maxArea != 0)
            {
                properties = properties.Where(p => p.PlotArea <= maxArea).ToList();
            }
            if (minPrice != 0)
            {
                properties = properties.Where(p => p.Price >= minPrice).ToList();
            }
            if (maxPrice != 0)
            {
                properties = properties.Where(p => p.Price <= maxPrice).ToList();
            }



            filterViewModel.Properties = properties;
            filterViewModel.Images = _context.Images
                            .Where(i => i.DisplayImage == true)
                            .ToList();

            return View(filterViewModel);

        }


    }
}