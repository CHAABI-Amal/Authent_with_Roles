using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authent.Controllers
{
    [Authorize] // Vérifie qu'un utilisateur est connecté
    public class RedirectController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RedirectController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);

                // Redirection en fonction du rôle
                if (roles.Contains("Admin"))
                    return RedirectToAction("Admin", "Roles");
                if (roles.Contains("Editor"))
                    return RedirectToAction("Editor", "Roles");
                if (roles.Contains("Manager"))
                    return RedirectToAction("Manager", "Roles");
                if (roles.Contains("Member"))
                    return RedirectToAction("Member", "Roles");
                if (roles.Contains("Contributor"))
                    return RedirectToAction("Contributor", "Roles");
                if (roles.Contains("Viewer"))
                    return RedirectToAction("Viewer", "Roles");
            }

            // Si l'utilisateur n'a pas de rôle ou n'est pas autorisé
            return RedirectToAction("AccessDenied", "Account");
        }
    }
}
