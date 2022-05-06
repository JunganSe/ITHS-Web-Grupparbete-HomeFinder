using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Image
    {
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        public bool DisplayImage { get; set; } = false;

        // Foreign keys
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
