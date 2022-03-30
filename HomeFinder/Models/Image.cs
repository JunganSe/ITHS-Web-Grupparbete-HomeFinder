using System.ComponentModel.DataAnnotations;

namespace HomeFinder.Models
{
    public class Image
    {
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        public string ImageAlt { get; set; }

        // Foreign keys
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
