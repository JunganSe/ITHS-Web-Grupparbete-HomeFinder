namespace HomeFinder.Models
{
    public class ExpressionOfInterest
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        // Foreign keys
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
