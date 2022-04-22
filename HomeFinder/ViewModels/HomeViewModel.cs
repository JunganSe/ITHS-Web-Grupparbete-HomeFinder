using HomeFinder.Models;
using System.Collections.Generic;

namespace HomeFinder.ViewModels
{
    public class HomeViewModel
    {
        public List<Property> Top3Viewed { get; set; }
        public List<Property> Top3Size { get; set; }
        public List<Property> Top3Price { get; set; }
        public List<Image> Images { get; set; }
    }
}
