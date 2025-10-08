using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BookNestWebApp.Models;

namespace BookNestWebApp.DAL
{
    public class BookRepository
    {
        // Connection string for BookNest DB 
        //private string connectionString = "Server=DESKTOP-PIU50MG;Database=BookNest;Integrated Security=True;Encrypt=False;";
        private string connectionString = "Server=DESKTOP-PIU50MG;Database=BookNest;User Id=BookNestAdmin;Password=StrongAdminPassword123!;Encrypt=False;";

        // Retrieves books using stored procedure with optional search and sort
        public List<Book> GetBooks(string search = "", string sort = "Title")
        {
            var books = new List<Book>();
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("sp_GetBooks", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Search", search);
                cmd.Parameters.AddWithValue("@Sort", sort);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            BookID = (int)reader["BookID"],
                            Title = reader["Title"]?.ToString() ?? "",
                            Price = (decimal)reader["Price"],
                            Genre = reader["Genre"]?.ToString() ?? "",
                            Stock = (int)reader["Stock"],
                            AuthorID = (int)reader["AuthorID"],
                            AuthorName = reader["AuthorName"]?.ToString() 
                        });
                    }
                }
            }
            return books;
        }

        // Adds a new book using stored procedure
        public void InsertBook(Book book)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("sp_InsertBook", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@AuthorID", book.AuthorID); 
                cmd.Parameters.AddWithValue("@Price", book.Price);
                cmd.Parameters.AddWithValue("@Genre", book.Genre);
                cmd.Parameters.AddWithValue("@Stock", book.Stock);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Updates a book using stored procedure
        public void UpdateBook(Book book)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("sp_UpdateBook", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookID", book.BookID);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@AuthorID", book.AuthorID); 
                cmd.Parameters.AddWithValue("@Price", book.Price);
                cmd.Parameters.AddWithValue("@Genre", book.Genre);
                cmd.Parameters.AddWithValue("@Stock", book.Stock);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Deletes a book by ID using stored procedure
        public void DeleteBook(int bookID)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("sp_DeleteBookByID", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookID", bookID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
