using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeFinder.Models
{
    public class Tenure
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }



        // Koppling
        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
