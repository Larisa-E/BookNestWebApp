namespace BookNestWebApp.Models
{
    // Represents a book in the BookNest database
    public class Book
    {
        public int BookID { get; set; }
        public string? Title { get; set; }
        public decimal Price { get; set; }
        public string? Genre { get; set; }
        public int Stock { get; set; }
        public int AuthorID { get; set; } 
        public string? AuthorName { get; set; } 
    }
}
