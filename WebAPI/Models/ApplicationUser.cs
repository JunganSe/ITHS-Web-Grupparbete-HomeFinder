using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [RegularExpression(@"^[A-Za-z -]+$")]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[A-Za-z -]+$")]
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        


        // Koppling
        public ICollection<ExpressionOfInterest> ExpressionOfInterests { get; set; } = new List<ExpressionOfInterest>();
        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
