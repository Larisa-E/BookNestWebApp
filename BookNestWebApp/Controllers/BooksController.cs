using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookNestWebApp.Models;
using BookNestWebApp.Services;
using System.Data.SqlClient;

public class BooksController : Controller
{
    private BookService service = new BookService();

    // List books with search/sort
    public ActionResult Index(string search = "", string sort = "Title")
    {
        ViewBag.Search = search;
        var books = service.GetBooks(search, sort);
        return View(books);
    }

    // Show create form
    //[Authorize(Roles = "BookNestStaff,BookNestAdmin")]
    public ActionResult Create() => View();

    // Handle create form post
    [HttpPost]
    //[Authorize(Roles = "BookNestStaff,BookNestAdmin")]
    public ActionResult Create(Book book)
    {
        if (!service.AuthorExists(book.AuthorID)) // validate the author
        {
            ModelState.AddModelError("AuthorID", "Selected Author does not exist.");
            return View(book);
        }
        service.InsertBook(book);
        return RedirectToAction("Index");
    }

    // Show edit form
    //[Authorize(Roles = "BookNestStaff,BookNestAdmin")]
    public ActionResult Edit(int id)
    {
        var book = service.GetBooks().Find(b => b.BookID == id);
        var authors = GetAuthors();
        ViewBag.AuthorList = new SelectList(authors, "AuthorID", "Name", book.AuthorID);
        return View(book);
    }

    // Handle edit form post
    [HttpPost]
    //[Authorize(Roles = "BookNestStaff,BookNestAdmin")]
    public IActionResult Edit(Book book)
    {
        var authors = GetAuthors();
        ViewBag.AuthorList = new SelectList(authors, "AuthorID", "Name", book.AuthorID);
        if (!service.AuthorExists(book.AuthorID))
        {
            ModelState.AddModelError("AuthorID", "Selected Author does not exist.");
            return View(book);
        }

        service.UpdateBook(book);
        return RedirectToAction("Index");
    }

    // Show delete confirmation
    //[Authorize(Roles = "BookNestAdmin")]
    public ActionResult Delete(int id)
    {
        var book = service.GetBooks().Find(b => b.BookID == id);
        return View(book);
    }

    // Handle delete confirmation
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken] // adds security
    //[Authorize(Roles = "BookNestAdmin")]
    public ActionResult DeleteConfirmed(int id)
    {
        service.DeleteBook(id);
        return RedirectToAction("Index");
    }

    // Helper to get authors
    private List<Author> GetAuthors()
    {
        var authors = new List<Author>();
        using (var conn = new SqlConnection("Server=DESKTOP-PIU50MG;Database=BookNest;Integrated Security=True;Encrypt=False;"))
        using (var cmd = new SqlCommand("SELECT AuthorID, Name FROM BookStore.Author", conn))
        {
            conn.Open();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    authors.Add(new Author
                    {
                        AuthorID = (int)reader["AuthorID"],
                        Name = reader["Name"].ToString()
                    });
                }
            }
        }
        return authors;
    }
}
