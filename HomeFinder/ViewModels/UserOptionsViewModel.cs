using System.ComponentModel.DataAnnotations;

namespace HomeFinder.ViewModels
{
    public class UserOptionsViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        [Display(Name = "Confirm new password")]
        public string ConfirmNewPassword { get; set; }
    }
}
