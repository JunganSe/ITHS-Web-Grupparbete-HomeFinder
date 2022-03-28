using System.ComponentModel.DataAnnotations;

namespace HomeFinder.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
