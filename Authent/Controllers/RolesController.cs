using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authent.Controllers
{
    [Authorize] // Vérifie qu'un utilisateur est connecté
    public class RolesController : Controller
    {
        // Page par défaut pour les rôles
        public IActionResult Index()
        {
            return View(); // Affiche une vue générale pour les rôles
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View(); // Retourne la vue Admin.cshtml
        }

        [Authorize(Roles = "Editor")]
        public IActionResult Editor()
        {
            return View(); // Retourne la vue Editor.cshtml
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Manager()
        {
            return View(); // Retourne la vue Manager.cshtml
        }

        [Authorize(Roles = "Member")]
        public IActionResult Member()
        {
            return View(); // Retourne la vue Member.cshtml
        }

        [Authorize(Roles = "Contributor")]
        public IActionResult Contributor()
        {
            return View(); // Retourne la vue Contributor.cshtml
        }

        [Authorize(Roles = "Viewer")]
        public IActionResult Viewer()
        {
            return View(); // Retourne la vue Viewer.cshtml
        }
    }
}
