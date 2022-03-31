using HomeFinder.Models;
using System.Collections.Generic;

namespace HomeFinder.ViewModels
{
    public class DisplayExpressionOfInterestsViewModel
    {
        public Dictionary<string, int> SaleStatuses { get; set; }
        public List<Property> Properties { get; set; }
    }
}