using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HomeFinder.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        


        // Koppling
        public ICollection<ExpressionOfInterest> ExpressionOfInterests { get; set; } = new List<ExpressionOfInterest>();
        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
