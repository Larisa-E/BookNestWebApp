using System.Diagnostics;
using BookNestWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using BookNestWebApp.Services;

namespace BookNestWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookService _bookService = new BookService();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var books = _bookService.GetBooks();
            var topBooks = books.Take(20).ToList(); // Show top 20 books
            ViewBag.BookCount = books.Count;
            return View(topBooks);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
