using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebAPI.ViewModels.Account
{
    public class PostAccountViewModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}