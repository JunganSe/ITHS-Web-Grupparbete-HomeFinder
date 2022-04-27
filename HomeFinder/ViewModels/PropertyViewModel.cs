using HomeFinder.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace HomeFinder.ViewModels
{
    public class PropertyViewModel
    {
        public Property Property { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Address Address { get; set; }
        public List<SaleStatus> SaleStatuses { get; set; } = new();
        public List<Tenure> Tenures { get; set; } = new();
        public List<PropertyType> PropertyTypes { get; set; } = new();
        public List<ApplicationUser> EstateAgents { get; set; } = new();
        public List<ApplicationUser> Users { get; set; } = new();
        public List<IFormFile> Images { get; set; } = new();
        public List<ExpressionOfInterest> ExpressionsOfInterest { get; set; } = new();
        public bool IsInterested { get; set; } = false;
        public List<string> ImageUrls { get; set; } = new();
    }
}