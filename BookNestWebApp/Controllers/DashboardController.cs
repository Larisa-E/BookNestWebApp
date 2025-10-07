using Microsoft.AspNetCore.Mvc;

namespace BookNestWebApp.Controllers
{
    public class DashboardController : Controller
    {
        // connect to the view for analytics
        public IActionResult Index()
        {
            return View();
        }
    }
}
