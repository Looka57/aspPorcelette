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
            var rolesToSeed = new[]
                    {
                        RoleConstants.Admin,
                        RoleConstants.Sensei,
                        RoleConstants.Adherent,
                        RoleConstants.Comite,
                        RoleConstants.Secretaire,
                        RoleConstants.Tresoriere,

                        // Anciens rôles conservés temporairement
                        RoleConstants.Student,
                        RoleConstants.Comptable
                    };

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
        // Dans AuthDbContextSeed.cs
        // Renommer la méthode pour plus de clarté
        public static async Task SeedAdminUserAsync(UserManager<User> userManager, IConfiguration configuration)
        {
            var adminEmail = configuration["AdminSeed:Email"] ?? "admin@porcelette.com"; // Email par défaut
            var adminPassword = configuration["AdminSeed:Password"] ?? "Test123!!"; // Mot de passe par défaut (IMPORTANT : sécurisez-le!)

            // 💥 Changement : Utiliser le rôle ADMIN pour cet utilisateur
            var adminRole = RoleConstants.Admin;

            // 1. Vérifier si l'utilisateur Admin existe déjà
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Bio = string.Empty,
                    Nom = "Super",
                    Prenom = "Admin",
                    RueEtNumero = string.Empty,
                    Ville = string.Empty,
                    CodePostal = string.Empty,
                    Telephone = string.Empty,
                    Grade = string.Empty,
                    PhotoUrl = string.Empty,

                    // Pour les DateTime, assurez-vous qu'elles ont une valeur valide si elles sont non-nullable
                    DateNaissance = new DateTime(1980, 1, 1),
                    DateAdhesion = DateTime.UtcNow,
                    DateRenouvellement = DateTime.UtcNow,
                    Statut = 1, // Si Statut est un int non-nullable
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    Console.WriteLine($"[SEED] Utilisateur Admin '{adminEmail}' créé.");

                    // 3. Lui attribuer le rôle ADMIN
                    if (await userManager.IsInRoleAsync(adminUser, adminRole) == false)
                    {
                        await userManager.AddToRoleAsync(adminUser, adminRole);
                        Console.WriteLine($"[SEED] Rôle '{adminRole}' attribué à l'utilisateur.");
                    }

                    // Laissez les claims commentés pour l'instant
                }
                else
                {
                    Console.WriteLine($"[ERREUR SEED] Échec de la création de l'Admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine($"[SEED] Utilisateur Admin '{adminEmail}' existe déjà.");
            }
        }
    }
}
