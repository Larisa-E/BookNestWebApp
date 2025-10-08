using Microsoft.AspNetCore.Mvc;
using BookNestWebApp.Services;
using System.Data.SqlClient;

namespace BookNestWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly BookService _bookService = new BookService();

        public IActionResult Index()
        {
            var books = _bookService.GetBooks();
            int bookCount = books.Count;

            // Example: Get order count (if you want to show order stats)
            int orderCount = 0;
            using (var conn = new SqlConnection("Server=DESKTOP-PIU50MG;Database=BookNest;User Id=BookNestAdmin;Password=StrongAdminPassword123!;Encrypt=False;"))
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM BookStore.[Order]", conn))
            {
                conn.Open();
                orderCount = (int)cmd.ExecuteScalar();
            }

            ViewBag.BookCount = bookCount;
            ViewBag.OrderCount = orderCount;
            return View(books);
        }
    }
}
