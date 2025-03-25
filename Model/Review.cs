namespace Project.Model
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public double Rating { get; set; }  // Example: 1-5 stars
        public string Comment { get; set; } = string.Empty;
    }
}
