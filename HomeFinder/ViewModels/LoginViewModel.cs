using System.ComponentModel.DataAnnotations;

namespace HomeFinder.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
