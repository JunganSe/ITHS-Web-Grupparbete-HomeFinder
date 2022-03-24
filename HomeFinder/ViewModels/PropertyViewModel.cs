using HomeFinder.Models;
using System.Collections.Generic;

namespace HomeFinder.ViewModels
{
    public class PropertyViewModel
    {
        public Property Property { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Adress Adress { get; set; }
        public List<SaleStatus> SaleStatuses { get; set; }
        public List<Tenure> Tenures { get; set; }
        public List<PropertyType> PropertyTypes { get; set; }
        public List<ApplicationUser> EstateAgents { get; set; }

    }
}
