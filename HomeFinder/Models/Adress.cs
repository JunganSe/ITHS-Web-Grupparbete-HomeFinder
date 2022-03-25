using System.ComponentModel.DataAnnotations;

namespace HomeFinder.Models
{
    public class Adress
    {
        public int Id { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Street Number")]
        public int StreetNumber { get; set; }

        [Required]
        [RegularExpression(@"^\d{5}$")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
