using ASPPorcelette.API.Constants;
using ASPPorcelette.API.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ASPPorcelette.API.Seed
{
    /// <summary>
    /// Classe statique pour l'initialisation des données d'identité (rôles et utilisateur Admin).
    /// </summary>
    public static class AuthDbContextSeed
    {
        // --- 1. SEEDING DES RÔLES ---
        
        /// <summary>
        /// Crée les rôles définis dans RoleConstants s'ils n'existent pas.
        /// </summary>
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var rolesToSeed = new[] { RoleConstants.Sensei, RoleConstants.Student };

            foreach (var roleName in rolesToSeed)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                    Console.WriteLine($"[SEED] Rôle '{roleName}' créé.");
                }
            }
        }

        // --- 2. SEEDING DE L'UTILISATEUR ADMINISTRATEUR (SENSEI) ---

        /// <summary>
        /// Crée un utilisateur Sensei par défaut s'il n'existe pas.
        /// </summary>
        public static async Task SeedSenseiUserAsync(UserManager<User> userManager, IConfiguration configuration)
        {
            // Récupérer les informations de l'Admin depuis la configuration (appsettings.json)
            var adminEmail = configuration["AdminSeed:Email"] ?? "admin@example.com";
            var adminPassword = configuration["AdminSeed:Password"] ?? "ComplexP@ssword123";
            var adminRole = RoleConstants.Sensei;

            // 1. Vérifier si l'utilisateur Admin existe déjà par son email
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var senseiUser = new User 
                { 
                    UserName = adminEmail, 
                    Email = adminEmail,
                    EmailConfirmed = true // Considérer l'admin comme confirmé
                    // Si vous avez d'autres propriétés (FirstName, LastName), ajoutez-les ici.
                };

                // 2. Créer l'utilisateur avec le mot de passe défini
                var result = await userManager.CreateAsync(senseiUser, adminPassword);

                if (result.Succeeded)
                {
                    Console.WriteLine($"[SEED] Utilisateur Sensei '{adminEmail}' créé.");
                    
                    // 3. Lui attribuer le rôle Sensei
                    if (await userManager.IsInRoleAsync(senseiUser, adminRole) == false)
                    {
                        await userManager.AddToRoleAsync(senseiUser, adminRole);
                        Console.WriteLine($"[SEED] Rôle '{adminRole}' attribué à l'utilisateur.");
                    }
                    
                    // Optionnel : Ajouter des Claims (utile si vous utilisez des politiques)
                    // await userManager.AddClaimAsync(senseiUser, new Claim("Role", "Admin"));
                }
                else
                {
                    Console.WriteLine($"[ERREUR SEED] Échec de la création du Sensei: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine($"[SEED] Utilisateur Sensei '{adminEmail}' existe déjà.");
            }
        }
    }
}
