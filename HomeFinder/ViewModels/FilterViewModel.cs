using HomeFinder.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeFinder.ViewModels
{
    public class FilterViewModel
    {
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int MinArea { get; set; }
        public int MaxArea { get; set; }
        public int MinNumberOfRooms { get; set; }
        public int MaxNumberOfRooms { get; set; }

        [Display(Name = "Property Type")]
        public List<PropertyType> PropertyTypes { get; set; }

        public List<Property> Properties { get; set; }
        public List<Tenure> Tenures { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public List<Image> Images { get; set; }


    }
}