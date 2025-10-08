using System.Collections.Generic;
using BookNestWebApp.DAL;
using BookNestWebApp.Models;
using System.Data.SqlClient;

namespace BookNestWebApp.Services
{
    public class BookService
    {
        private BookRepository repo = new BookRepository();
        private string connectionString = "Server=DESKTOP-PIU50MG;Database=BookNest;User Id=BookNestAdmin;Password=StrongAdminPassword123!;Encrypt=False;"; // SQL authentication
        //private string connectionString = "Server=DESKTOP-PIU50MG;Database=BookNest;Integrated Security=True;Encrypt=False;"; // Default Connection

        public BookService() { }

        // Gets all books, applies business rules if needed
        public List<Book> GetBooks(string search = "", string sort = "Title")
        {
            return repo.GetBooks(search, sort);
        }

        public bool InsertBook(Book book)
        {
            if (!AuthorExists(book.AuthorID)) // validate the author
                return false; 

            repo.InsertBook(book);
            return true;
        }

        public bool UpdateBook(Book book)
        {
            if (!AuthorExists(book.AuthorID))
                return false;

            repo.UpdateBook(book);
            return true;
        }

        public void DeleteBook(int bookID)
        {
            repo.DeleteBook(bookID);
        }

        public bool AuthorExists(int authorId)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM BookStore.Author WHERE AuthorID = @AuthorID", conn))
            {
                cmd.Parameters.AddWithValue("@AuthorID", authorId);
                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        // Get author name by ID
        public string? GetAuthorName(int authorId)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT Name FROM BookStore.Author WHERE AuthorID = @AuthorID", conn))
            {
                cmd.Parameters.AddWithValue("@AuthorID", authorId);
                conn.Open();
                var result = cmd.ExecuteScalar();
                return result == null ? null : result.ToString();
            }
        }
    }
}
