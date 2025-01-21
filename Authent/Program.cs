using Authent.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authent
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuration de la base de données
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                         ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(connectionString, sqlOptions =>
             {
                 sqlOptions.EnableRetryOnFailure(
                     maxRetryCount: 10, // Nombre maximal de tentatives avant l'échec
                     maxRetryDelay: TimeSpan.FromSeconds(20), // Délai maximal entre les tentatives
                     errorNumbersToAdd: null); // Nombres d'erreurs supplémentaires à considérer comme transitoires
             }));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Configuration d'Identity
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Pipeline HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Roles/AccessDenied"); // Redirige vers la vue AccessDenied en cas d'erreur
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Routes
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Redirect}/{action=Index}/{id?}"); // Par défaut, redirige vers RedirectController

            app.MapRazorPages();

            // Initialisation des rôles et de l'utilisateur admin
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                // Ajouter les rôles nécessaires et créer l'admin user
                await DataSeeder.SeedRolesAndAdminUser(services);
            }

            app.Run();
        }
    }
}
