using System.ComponentModel.DataAnnotations;

namespace HomeFinder.ViewModels
{
    public class UserOptionsViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z -]+$", ErrorMessage ="Only letters in your First Name, you donkey!")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z -]+$", ErrorMessage = "Only letters in your Last Name, you donkey!")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
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
