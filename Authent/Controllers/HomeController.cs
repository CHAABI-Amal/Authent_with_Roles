using Microsoft.AspNetCore.Mvc;

namespace Authent.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Affiche Views/Home/Index.cshtml
        }

        public IActionResult Privacy()
        {
            return View(); // Affiche Views/Home/Privacy.cshtml
        }

        public IActionResult About()
        {
            return View(); // Affiche Views/Home/About.cshtml
        }
    }
}
