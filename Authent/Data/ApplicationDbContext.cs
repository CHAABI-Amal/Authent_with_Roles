// Importation des bibliothèques nécessaires pour la gestion des identités et l'accès à la base de données
using Microsoft.AspNetCore.Identity; // Fournit des fonctionnalités pour gérer les identités (utilisateurs, rôles, etc.)
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Fournit une implémentation d'Entity Framework Core pour Identity
using Microsoft.EntityFrameworkCore; // Fournit des outils pour interagir avec la base de données via Entity Framework Core

// Déclaration de l'espace de noms du projet
namespace Authent.Data
{
    // Classe principale du contexte de la base de données héritant de IdentityDbContext
    public class ApplicationDbContext : IdentityDbContext
    {
        // Constructeur pour initialiser le contexte de la base de données avec des options spécifiques
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) // Appelle le constructeur de la classe de base avec les options fournies
        {
        }
    }

    // Classe pour ajouter des données initiales à la base de données (comme les rôles et un utilisateur administrateur)
    public class DataSeeder
    {
        // Méthode statique pour insérer les rôles et l'utilisateur administrateur
        public static async Task SeedRolesAndAdminUser(IServiceProvider serviceProvider)
        {
            // Récupération des services nécessaires : gestionnaire de rôles et gestionnaire d'utilisateurs
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>(); // Gère les rôles
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>(); // Gère les utilisateurs

            // Définition des paramètres de l'administrateur
            string adminRoleName = "Admin"; // Nom du rôle administrateur
            string adminEmail = "admin@example.com"; // Email de l'administrateur
            string adminPassword = "Admin@1234"; // Mot de passe de l'administrateur (utiliser un mot de passe sécurisé en production)

            // Création des rôles nécessaires
            var roles = new[] { "Admin", "Manager", "Editor", "Contributor", "Viewer", "Member" }; // Liste des rôles à créer

            // Boucle pour vérifier et créer chaque rôle s'il n'existe pas
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role)) // Vérifie si le rôle existe déjà
                {
                    await roleManager.CreateAsync(new IdentityRole(role)); // Crée le rôle s'il n'existe pas
                }
            }

            // Vérifie si l'utilisateur administrateur existe déjà
            var adminUser = await userManager.FindByEmailAsync(adminEmail); // Recherche l'utilisateur par email
            if (adminUser == null) // Si l'utilisateur n'existe pas
            {
                // Crée un nouvel utilisateur administrateur
                adminUser = new IdentityUser
                {
                    UserName = adminEmail, // Définit le nom d'utilisateur comme l'email
                    Email = adminEmail, // Définit l'email
                    EmailConfirmed = true // Confirme l'email par défaut
                };

                // Ajoute l'utilisateur administrateur avec le mot de passe défini
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded) // Vérifie si la création a réussi
                {
                    // Ajoute l'utilisateur administrateur au rôle "Admin"
                    await userManager.AddToRoleAsync(adminUser, adminRoleName);
                }
            }
        }
    }
}
